using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface ICommonRepository
    {
        Task<List<DropdownResponse>> GetAllDriverVehiclesDropdown();
    }
}
