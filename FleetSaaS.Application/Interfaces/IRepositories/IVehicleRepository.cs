using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface IVehicleRepository
    {
        Task<VehicleResponse> GetAllVehicles(PagedRequest pagedRequest);
        Task DeleteVehicle(Guid vehicleId);
        void AddVehicle(VehicleRequest vehicleRequest);
        void UpdateVehicle(VehicleRequest vehicleRequest);
    }
}
