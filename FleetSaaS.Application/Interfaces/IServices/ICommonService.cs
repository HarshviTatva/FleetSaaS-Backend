using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Enum;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface ICommonService
    {
        string GenerateRandomPassword(int length);
        Task<List<DropdownResponse>> GetAllDriverVehiclesDropdown();
        Task<string> GetTripStatusMessage(TripStatus status);
    }
}
