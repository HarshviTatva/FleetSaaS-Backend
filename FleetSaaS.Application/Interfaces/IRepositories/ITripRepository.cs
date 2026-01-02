using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface ITripRepository
    {
        Task<TripResponse> GetAllTrips(PagedRequest pagedRequest);
        Task UpdateTrip(Trip trip);
        Task AddTrip(Trip trip);
        Task CancelTrip(Guid tripId);
        Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest);
        Task UnAssignTripToDriver(Guid tripId);
    }
}
