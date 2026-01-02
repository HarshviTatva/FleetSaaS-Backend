using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;

namespace FleetSaaS.Application.Services
{
    public class TripService(ITripRepository _tripRepository,IMapper _mapper) : ITripService
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
                await _tripRepository.AddTrip(trip);
            }
            else
            {
                trip.UpdatedAt = DateTime.UtcNow;
                await _tripRepository.UpdateTrip(trip);
            }
            return trip.Id;
        }

        public async Task CancelTrip(Guid id)
        {
            await _tripRepository.CancelTrip(id);
        }

        public async Task AssignTripToDriver(AssignTripDriverRequest assignTripDriverRequest)
        {
            await _tripRepository.AssignTripToDriver(assignTripDriverRequest);
        }

        public async Task UnAssignTripToDriver(Guid tripId)
        {
            await _tripRepository.UnAssignTripToDriver(tripId);
        }
    }
}
