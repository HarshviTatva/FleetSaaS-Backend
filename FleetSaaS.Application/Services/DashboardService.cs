using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Helper;
using Microsoft.AspNetCore.Http;

namespace FleetSaaS.Application.Services
{
    public class DashboardService(
        IGenericRepository<Trip> _tripRepository,
        IGenericRepository<Driver> _driverRepository,
        IGenericRepository<Vehicle> _vehicleRepository,
        IHttpContextAccessor _httpContextAccessor
        ) : IDashboardService
    {
        public async Task<DashboardResponse> GetCompanyUserDashboardDetails(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new Exception(MessageConstants.DATA_RETRIEVAL_FAILED);

            return new DashboardResponse
            {
                TotalVehicleCount = await _vehicleRepository.CountAsync(v =>
                    v.CompanyId == companyId && !v.IsDeleted),

                TotalActiveVehicleCount = await _vehicleRepository.CountAsync(v =>
                    v.CompanyId == companyId && v.IsActive && !v.IsDeleted),

                TotalDriversCount = await _driverRepository.CountAsync(d =>
                    d.CompanyId == companyId && !d.IsDeleted),

                TotalTrips = await _tripRepository.CountAsync(t =>
                    t.CompanyId == companyId && !t.IsDeleted),

                TotalPlannedTrips = await _tripRepository.CountAsync(t =>
                    t.CompanyId == companyId &&
                    t.Status == TripStatus.Planned &&
                    !t.IsDeleted),

                TotalOngoingTrips = await _tripRepository.CountAsync(t =>
                    t.CompanyId == companyId &&
                    (t.Status == TripStatus.Assigned ||
                     t.Status == TripStatus.Accepted ||
                     t.Status == TripStatus.Started) &&
                    !t.IsDeleted),

                TotalCompletedTrips = await _tripRepository.CountAsync(t =>
                    t.CompanyId == companyId &&
                    t.Status == TripStatus.Completed &&
                    !t.IsDeleted),

                LicenseExpiryAlerts = await GetExpiryAlerts(companyId)
            };
        }

        public async Task<List<ExpiryAlertDTO>> GetExpiryAlerts(Guid companyId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var next30Days = today.AddDays(30);

            var driverAlerts = await _driverRepository.FindAsync(
                d =>
                    d.CompanyId == companyId &&
                    !d.IsDeleted &&
                    d.LicenseExpiry != null &&
                    d.LicenseExpiry <= next30Days,
                d => new ExpiryAlertDTO
                {
                    Type = "DriverLicense",
                    Reference = d.LicenseNumber,
                    ExpiryDate =
                        d.LicenseExpiry!.Value.ToDateTime(TimeOnly.MinValue),
                    IsExpired = d.LicenseExpiry <= today
                });

            var vehicleAlerts = await _vehicleRepository.FindAsync(
                v =>
                    v.CompanyId == companyId &&
                    !v.IsDeleted &&
                    v.InsuranceExpiryDate != null &&
                    v.InsuranceExpiryDate <= next30Days,
                v => new ExpiryAlertDTO
                {
                    Type = "VehicleInsurance",
                    Reference = v.LicensePlate,
                    ExpiryDate =
                        v.InsuranceExpiryDate!.Value.ToDateTime(TimeOnly.MinValue),
                    IsExpired = v.InsuranceExpiryDate < today
                });

            return driverAlerts
                .Concat(vehicleAlerts)
                .OrderBy(a => a.ExpiryDate)
                .Take(5)
                .ToList();
        }

        public async Task<DispatcherDashboardResponse> GetDispatcherDashboardDetails(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new Exception(MessageConstants.DATA_RETRIEVAL_FAILED);

            int totalDrivers = await _driverRepository.CountAsync(d =>
                d.CompanyId == companyId && !d.IsDeleted);

            int availableDrivers = await _driverRepository.CountAsync(d =>
                d.CompanyId == companyId &&
                !d.IsDeleted &&
                d.IsAvailable);

            int onTripDrivers = await _driverRepository.CountAsync(d =>
                d.CompanyId == companyId &&
                !d.IsDeleted &&
                !d.IsAvailable);

            int activeTrips = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                (t.Status == TripStatus.Assigned ||
                 t.Status == TripStatus.Accepted ||
                 t.Status == TripStatus.Started) &&
                !t.IsDeleted);

            int upcomingTrips = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                t.Status == TripStatus.Planned &&
                !t.IsDeleted);

            var activeTripList = await _tripRepository.FindAsync(
                t =>
                    t.CompanyId == companyId &&
                    (t.Status == TripStatus.Assigned ||
                     t.Status == TripStatus.Accepted ||
                     t.Status == TripStatus.Started) &&
                    !t.IsDeleted,
                t => new ActiveTripResponse
                {
                    TripNo = GenerateTripNameHelper.GenerateTripName(t.Origin,t.Destination,t.CreatedAt),
                    Status = t.Status.ToString(),
                    DriverName = t.VehicleAssignment.Driver!.User.UserName,
                    Vehicle = t.VehicleAssignment!.Vehicle.LicensePlate
                });

            var upcomingTripList = await _tripRepository.FindAsync(
                t =>
                    t.CompanyId == companyId &&
                    t.Status == TripStatus.Planned &&
                    !t.IsDeleted,
                t => new UpcomingTripResponse
                {
                    Id= t.Id,
                    TripNo = GenerateTripNameHelper.GenerateTripName(t.Origin, t.Destination, t.CreatedAt),
                    Time = t.ScheduledAt!.Value.ToString("hh:mm tt"),
                    Route = $"{t.Origin} → {t.Destination}"
                });

            return new DispatcherDashboardResponse
            {
                ActiveTrips = activeTrips,
                UpcomingTrips = upcomingTrips,

                Drivers = new DriverSummaryResponse
                {
                    Total = totalDrivers,
                    Available = availableDrivers,
                    OnTrip = onTripDrivers,
                    Unavailable = totalDrivers - availableDrivers - onTripDrivers
                },

                Trips = activeTripList.Take(5).ToList(),
                Upcoming = upcomingTripList.Take(5).ToList()
            };
        }

        public async Task<DriverDashboardResponse> GetDriverDashboardDetails(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new Exception(MessageConstants.DATA_RETRIEVAL_FAILED);

            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst("UserId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException(MessageConstants.DATA_RETRIEVAL_FAILED);

            Guid driverId = await _driverRepository.FindFirstAsync<Guid>(d => d.UserId == userId && !d.IsDeleted,d => d.Id,false);

            if (driverId == Guid.Empty)
                throw new Exception(MessageConstants.DATA_RETRIEVAL_FAILED);

            int totalTrips = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                t.VehicleAssignment!.DriverId == driverId &&
                !t.IsDeleted);

            int completedTripsCount = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                t.VehicleAssignment!.DriverId == driverId &&
                t.Status == TripStatus.Completed &&
                !t.IsDeleted);

            var todayTrip = await _tripRepository.FindAsync(
                t =>
                    t.CompanyId == companyId &&
                    t.VehicleAssignment!.DriverId == driverId &&
                    (t.Status == TripStatus.Assigned ||
                     t.Status == TripStatus.Accepted ||
                     t.Status == TripStatus.Started) &&
                    !t.IsDeleted,
                t => new DriverTodayTrip
                {
                    TripCode = GenerateTripNameHelper.GenerateTripName(
                        t.Origin, t.Destination, t.CreatedAt),
                    Origin = t.Origin,
                    Destination = t.Destination,
                    Status = t.Status,
                    ScheduledAt = t.ScheduledAt
                });

            int acceptedTripsCount = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                t.VehicleAssignment!.DriverId == driverId &&
                t.Status == TripStatus.Accepted &&
                !t.IsDeleted);

            int cancelledTripsCount = await _tripRepository.CountAsync(t =>
                t.CompanyId == companyId &&
                t.VehicleAssignment!.DriverId == driverId &&
                t.Status == TripStatus.Cancelled &&
                !t.IsDeleted);

            var completedTrips = await _tripRepository.FindAsync(
                t =>
                    t.CompanyId == companyId &&
                    t.VehicleAssignment!.DriverId == driverId &&
                    t.Status == TripStatus.Completed &&
                    !t.IsDeleted,
                t => new DriverCompletedTrips
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destination = t.Destination,
                    CompletedAt = t.UpdatedAt ?? t.CreatedAt
                });

            return new DriverDashboardResponse
            {
                TotalTrips = totalTrips,
                AcceptedTripsCount = acceptedTripsCount,
                CancelledTripsCount = cancelledTripsCount,
                CompletedTripsCount = completedTripsCount,
                TodayTrips = todayTrip,
                CompletedTrips = completedTrips
                    .OrderByDescending(x => x.CompletedAt)
                    .Take(5)
                    .ToList()
            };
        }
    }
}
