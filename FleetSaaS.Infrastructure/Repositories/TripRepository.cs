using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Helper;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace FleetSaaS.Infrastructure.Repositories
{
    public class TripRepository(ITenantProvider _tenantProvider,ApplicationDbContext _dbContext, IHttpContextAccessor _httpContextAccessor) : ITripRepository
    {
        public async Task<TripResponse> GetAllTrips(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;

            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst("UserId")?.Value;

            var roleClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst("RoleId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException(MessageConstants.NO_RECORD_FOUND);

            if (!Enum.TryParse<RoleType>(roleClaim, out var role))
                throw new UnauthorizedAccessException("Invalid role");

            IQueryable<TripDTO> query =
                from t in _dbContext.Trips.AsNoTracking()

                join va in _dbContext.VehicleAssignments
                    on t.VehicleAssignmentId equals va.Id into vaGroup
                from va in vaGroup.DefaultIfEmpty()

                join v in _dbContext.Vehicles
                    on va.VehicleId equals v.Id into vGroup
                from v in vGroup.DefaultIfEmpty()

                join d in _dbContext.Drivers
                    on va.DriverId equals d.Id into dGroup
                from d in dGroup.DefaultIfEmpty()

                join u in _dbContext.Users
                    on d.UserId equals u.Id into uGroup
                from u in uGroup.DefaultIfEmpty()

                select new TripDTO
                {
                    Id = t.Id,
                    Name = GenerateTripNameHelper.GenerateTripName(t.Origin, t.Destination, t.CreatedAt),
                    Origin = t.Origin,
                    Destination = t.Destination,
                    Description = t.Description,
                    ScheduledAt = t.ScheduledAt != null
                        ? t.ScheduledAt.Value.ToString()
                        : null,
                    Status = t.Status,
                    VehicleAssignmentId = va != null ? va.Id : null,
                    ScheduleDateFilter = t.ScheduledAt,
                    VehicleDriverName =
                        (v != null && u != null)
                            ? $"{v.Model} - {v.LicensePlate} | {u.UserName}"
                            : null,

                    DriverUserId = u != null ? u.Id : null,
                    DistanceCovered = t.DistanceCovered,
                    CreatedAt = t.CreatedAt
                };
            if (role == RoleType.Driver)
            {
                if (pagedRequest.ShowCompletedRecords is true)
                {
                    var today = DateTime.UtcNow.Date;

                    query = query.Where(t =>
                        t.VehicleAssignmentId != null &&
                        t.DriverUserId == userId &&
                        t.ScheduleDateFilter < today 
                    );
                }
                else
                {
                    query = query.Where(t =>
                    t.VehicleAssignmentId != null &&
                    t.DriverUserId == userId && t.Status != TripStatus.Completed);
                }
            }

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();

                query = query.Where(t =>
                    t.Origin.ToLower().Contains(search) ||
                    t.Destination.ToLower().Contains(search));
            }

            if (pagedRequest.Status > 0)
            {
                query = query.Where(t =>
                    t.Status == (TripStatus)pagedRequest.Status);
            }

            if (DateTime.TryParse(pagedRequest.Date, out _))
            {
                query = query.Where(v =>
                    !string.IsNullOrWhiteSpace(v.ScheduledAt) &&
                    v.ScheduledAt.StartsWith(pagedRequest.Date)
                );
            }

            query = pagedRequest.SortBy?.ToLower() switch
            {
                "origin" => pagedRequest.SortDirection == "asc"
                    ? query.OrderBy(t => t.Origin)
                    : query.OrderByDescending(t => t.Origin),

                "destination" => pagedRequest.SortDirection == "asc"
                    ? query.OrderBy(t => t.Destination)
                    : query.OrderByDescending(t => t.Destination),

                _ => query.OrderByDescending(t => t.Id)
            };

            var totalCount = await query.CountAsync();

            var trips = await query
                .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .ToListAsync();

            return new TripResponse
            {
                Trips = trips,
                CompanyId = companyId,
                TotalCount = totalCount,
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize
            };
        }

        public async Task AddTrip(Trip trip)
        {
            trip.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.Trips.AddAsync(trip);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTrip(Trip trip)
        {
            trip.CompanyId = _tenantProvider.CompanyId;
            _dbContext.Trips.Update(trip);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelTrip(CancelTripRequest cancelTripRequest)
        {
            var trip = await _dbContext.Trips
                .FirstOrDefaultAsync(u => u.Id == cancelTripRequest.Id);

            if (trip != null)
            {
                trip.Status = TripStatus.Cancelled;
                trip.IsDeleted = true;
                trip.CancelReason = cancelTripRequest.CancelReason;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest)
        {
            Trip? tripDetail = await _dbContext.Trips.FirstOrDefaultAsync(x => x.Id == assignTripDriverRequest.Id && !x.IsDeleted && x.Status!=TripStatus.Cancelled);
            if (tripDetail != null)
            {
                tripDetail.Status = TripStatus.Assigned;
                tripDetail.VehicleAssignmentId = assignTripDriverRequest.VehicleAssignmentId;
                tripDetail.UpdatedAt = DateTime.UtcNow;
                tripDetail.UpdatedBy = _tenantProvider.CompanyId;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnAssignTripToDriver(Guid tripId)
        {
            Trip? tripDetail = await _dbContext.Trips.FirstOrDefaultAsync(x => x.Id == tripId && !x.IsDeleted && x.Status!=TripStatus.Started);
            if (tripDetail != null)
            {
                tripDetail.Status = TripStatus.Planned;
                tripDetail.VehicleAssignmentId = null;
                tripDetail.UpdatedAt = DateTime.UtcNow;
                tripDetail.UpdatedBy = _tenantProvider.CompanyId;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
