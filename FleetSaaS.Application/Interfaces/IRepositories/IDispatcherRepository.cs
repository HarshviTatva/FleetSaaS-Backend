using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface IDispatcherRepository
    {
        Task<DispatcherResponse> GetAllDispatchers(PagedRequest pagedRequest);
        Task DeleteDispatcher(Guid dispatcherId);
    }
}
