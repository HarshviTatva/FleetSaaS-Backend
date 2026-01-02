using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface ICommonService
    {
        string GenerateRandomPassword(int length);
        Task<List<DropdownResponse>> GetAllDriverVehiclesDropdown();
    }
}
