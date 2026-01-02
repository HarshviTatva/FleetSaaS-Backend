using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Infrastructure.Common;
using System.Security.Cryptography;

namespace FleetSaaS.Application.Services
{
    public class CommonService(ICommonRepository _commonRepository) : ICommonService
    {
        public string GenerateRandomPassword(int length)
        {
            const string validChars = Fields.RANDOM_STRING;
            string password = RandomNumberGenerator.GetString(validChars, length);
            return password;
        }

        public async Task<List<DropdownResponse>> GetAllDriverVehiclesDropdown()
        {
            return await _commonRepository.GetAllDriverVehiclesDropdown();
        }
    }
}
