using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Enum;
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

        public async Task<string> GetTripStatusMessage(TripStatus status)
        {
            return status switch
            {
                TripStatus.Planned => string.Format(MessageConstants.PLANNED_MESSAGE, "Trip"),
                TripStatus.Assigned => string.Format(MessageConstants.ASSIGNED_MESSAGE, "Trip"),
                TripStatus.Accepted => string.Format(MessageConstants.ACCEPTED_MESSAGE, "Trip"),
                TripStatus.Started => string.Format(MessageConstants.STARTED_MESSAGE, "Trip"),
                TripStatus.Completed => string.Format(MessageConstants.COMPLETED_MESSAGE, "Trip"),
                TripStatus.Cancelled => string.Format(MessageConstants.CANCELLED_MESSAGE, "Trip"),
                _ => MessageConstants.DATA_RETRIEVED
            };
        }
    }
}
