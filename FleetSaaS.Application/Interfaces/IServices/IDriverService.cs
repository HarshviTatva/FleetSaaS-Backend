using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IDriverService
    {
        Task<Guid> AddEditDriver(DriverUserRequest driverRequest);
        Task<DriverResponse> GetAllDrivers(PagedRequest request);
        Task DeleteDriver(Guid Id);
    }
}
