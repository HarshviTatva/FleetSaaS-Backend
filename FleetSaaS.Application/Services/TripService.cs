using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Document;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Helper;
using QuestPDF.Fluent;

namespace FleetSaaS.Application.Services
{
    public class TripService(ITripRepository _tripRepository,IMapper _mapper, IGenericRepository<Trip> _genericRepo) : ITripService
    {
        public async Task<TripResponse> GetAllTrips(PagedRequest request)
        {
            return await _tripRepository.GetAllTrips(request);
        }

        public async Task<Guid> AddEditTrip(TripRequest tripRequest)
        {
            Trip trip = _mapper.Map<Trip>(tripRequest);
            
            if (tripRequest.Id  == null)
            {
                trip.CreatedAt = DateTime.UtcNow;
                trip.Status = TripStatus.Planned;
                trip.CompanyId = _genericRepo.GetCompanyId();
                await _genericRepo.AddAsync(trip);
                //await _tripRepository.AddTrip(trip);
            }
            else
            {
                trip.UpdatedAt = DateTime.UtcNow;
                await _tripRepository.UpdateTrip(trip);
            }
            return trip.Id;
        }

        public async Task CancelTrip(CancelTripRequest cancelTripRequest)
        {
            await _tripRepository.CancelTrip(cancelTripRequest);
        }

        public async Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest)
        {
            await _tripRepository.AssignTripToDriver(assignTripDriverRequest);
        }

        public async Task UnAssignTripToDriver(Guid tripId)
        {
            await _tripRepository.UnAssignTripToDriver(tripId);
        }

        public async Task ChangeTripStatus(Guid id, TripStatus status, long? distanceCovered)
        {
            Guid companyId = _genericRepo.GetCompanyId();

            Trip? trip = await _genericRepo.GetByIdAsync(id);

            if (trip == null)
                throw new KeyNotFoundException(MessageConstants.NO_RECORD_FOUND);

            trip.Status = status;
            trip.UpdatedAt = DateTime.UtcNow;
            trip.UpdatedBy = companyId;
            if(status== TripStatus.Completed)
            {
                trip.DistanceCovered = distanceCovered;
            }
            await _genericRepo.UpdateAsync(trip);
        }

        public async Task<byte[]> ExportTripsToCsvAsync(PagedRequest request)
        {
            TripResponse tripResponse = await _tripRepository.GetAllTrips(request);

            var exportData = _mapper.Map<IEnumerable<TripExportDTO>>(
                tripResponse.Trips
            );

            return CsvExportHelper.GenerateCsv(exportData);
        }

        public async Task<byte[]> GeneratePdfReport(Guid tripId)
        {
            var trip = await _genericRepo.FindFirstAsync(
             t => t.Id == tripId && !t.IsDeleted,
             t => new TripPdfDTO
             {
                 TripCode = GenerateTripNameHelper.GenerateTripName(
                     t.Origin, t.Destination, t.CreatedAt),

                 Origin = t.Origin,
                 Destination = t.Destination,
                 Description = t.Description,

                 ScheduledAt = t.ScheduledAt.ToString(),
                 StatusName = t.Status.ToString(),
                 Status = t.Status,
                 DistanceCovered = t.DistanceCovered,

                 Model = t.VehicleAssignment == null
                     ? null
                     : t.VehicleAssignment.Vehicle.Model,

                 Make = t.VehicleAssignment == null
                     ? null
                     : t.VehicleAssignment.Vehicle.Make,

                 VehicleNumber = t.VehicleAssignment == null
                     ? null
                     : t.VehicleAssignment.Vehicle.LicensePlate,

                 DriverName = t.VehicleAssignment == null
                              || t.VehicleAssignment.Driver == null
                              || t.VehicleAssignment.Driver.User == null
                     ? null
                     : t.VehicleAssignment.Driver.User.UserName,

                 Completed_At = t.Status == TripStatus.Completed
                     ? t.UpdatedAt.ToString()
                     : null,

                 CancelReason = t.Status == TripStatus.Cancelled
                     ? t.CancelReason
                     : null
             });


            if (trip == null)
                throw new AppDomainUnloadedException(MessageConstants.NO_RECORD_FOUND);

            var document = new TripDocument(trip);
            return document.GeneratePdf();
        }
    }
}
