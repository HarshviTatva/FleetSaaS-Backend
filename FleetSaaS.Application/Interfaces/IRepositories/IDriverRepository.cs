using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface IDriverRepository
    {
        Task<bool> ExistsByLicenseNumberAsync(string licenseNumber);
        Task<DriverResponse> GetAllDrivers(PagedRequest pagedRequest);
        Task AddDriver(Driver driver);
        Task UpdateDriver(Driver driverUser);
        Task DeleteDriver(Guid driverId);
    }
}
