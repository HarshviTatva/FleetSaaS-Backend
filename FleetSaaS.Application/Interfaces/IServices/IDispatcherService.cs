using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IDispatcherService
    {
        Task DeleteDispatcher(Guid Id);
        Task<DispatcherResponse> GetAllDispatchers(PagedRequest request);
        Task<Guid> AddEditDispatcher(UserRequest userRequest);
    }
}
