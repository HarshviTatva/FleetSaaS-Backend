using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface IVehicleRepository
    {
        Task<VehicleResponse> GetAllVehicles(PagedRequest pagedRequest);
        Task DeleteVehicle(Guid vehicleId);
        Task AddVehicle(Vehicle vehicle);
        Task UpdateVehicle(Vehicle vehicle);
        Task<bool> ExistsByVinAsync(string vin, Guid? vehicleId = null);
        Task<bool> ExistsByLicensePlateAsync(string vin, Guid? vehicleId = null);
    }
}
