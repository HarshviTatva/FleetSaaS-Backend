using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Enum;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface ITripService
    {
        Task<TripResponse> GetAllTrips(PagedRequest request);
        Task<Guid> AddEditTrip(TripRequest tripRequest);
        Task CancelTrip(CancelTripRequest cancelTripRequest);
        Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest);
        Task UnAssignTripToDriver(Guid tripId);
        Task ChangeTripStatus(Guid id, TripStatus status, long? distanceCovered);
        Task<byte[]> ExportTripsToCsvAsync(PagedRequest request);
        Task<byte[]> GeneratePdfReport(Guid tripId);
    }
}
