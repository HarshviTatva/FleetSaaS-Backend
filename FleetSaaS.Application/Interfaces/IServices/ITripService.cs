using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface ITripService
    {
        Task<TripResponse> GetAllTrips(PagedRequest request);
        Task<Guid> AddEditTrip(TripRequest tripRequest);
        Task CancelTrip(Guid id);
        Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest);
        Task UnAssignTripToDriver(Guid tripId);
    }
}
