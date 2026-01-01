using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Infrastructure.Common;
using System.Security.Cryptography;

namespace FleetSaaS.Application.Services
{
    public class CommonService : ICommonService
    {
        public string GenerateRandomPassword(int length)
        {
            const string validChars = Fields.RANDOM_STRING;
            string password = RandomNumberGenerator.GetString(validChars, length);
            return password;
        }
    }
}
