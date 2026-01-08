using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IVehicleService
    {
        Task<VehicleResponse> GetAllVehicles(PagedRequest request);
        Task DeleteVehicle(Guid Id);
        Task<Guid> AddEditVehicle(VehicleRequest vehicleRequest);
        Task<List<DropdownResponse>> GetAllVehiclesDropdown();
        Task<Guid> AssignVehicleToDriver(AssignVehicleRequest assignVehicleRequest);
        Task<Guid> ReAssignVehicleToDriver(AssignVehicleRequest assignVehicleRequest);
        Task UnAssignVehicleToDriver(Guid id);
        Task<byte[]> ExportVehicleToCsvAsync(PagedRequest request);
    }
}
