using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;

namespace FleetSaaS.Application.Services
{
    public class UserService(IUserRepository userRepository, ICompanyRepository companyRepository) : IUserService
    {
 
    }
}
