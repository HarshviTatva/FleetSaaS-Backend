using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IVehicleService
    {
        Task<VehicleResponse> GetAllVehicles(PagedRequest request);
        Task DeleteVehicle(Guid Id);
        Task<Guid> AddEditVehicle(VehicleRequest vehicleRequest);
    }
}
