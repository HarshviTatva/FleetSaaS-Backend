using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IVehicleService
    {
        Task<VehicleResponse> GetAllVehicles(PagedRequest request);
        Task DeleteVehicle(Guid Id);
        Task<IActionResult> AddEditVehicle(VehicleRequest vehicleRequest);
    }
}
