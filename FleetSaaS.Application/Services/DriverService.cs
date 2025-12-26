using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.Application.Services
{
    public class DriverService(
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordHasher<User> _passwordHasher,
        IMapper _mapper,
        IDriverRepository driverRepository
        ) : IDriverService
    {
        public async Task<Guid> AddEditDriver(DriverUserRequest driverRequest)
        {
            if (driverRequest?.Id==null)
            {
                if (await userRepository.ExistsByEmailAsync(driverRequest.Email))
                    throw new ApplicationException(MessageConstants.USER_EXISTS);

                if (await driverRepository.ExistsByLicenseNumberAsync(driverRequest.LicenseNumber))
                    throw new ApplicationException(DriverMessages.LICENSE_NO_EXISTS);

                var userRequest = _mapper.Map<User>(driverRequest);
                userRequest.Password = _passwordHasher.HashPassword(
                      userRequest,
                      driverRequest.UserName+"@123"
                  );
                userRequest.RoleId = (int)RoleType.Driver;
                await userRepository.AddTenantAsUser(userRequest);

                //email by smtp
                await emailService.SendAsync(
                       driverRequest.Email,
                       "Your Driver Account Created",
                       $@"
                          <h3>Welcome {driverRequest.UserName}</h3>
                          <p>Your driver account has been created.</p>
                          <p><b>Username:</b> {driverRequest.UserName}</p>
                          <p><b>Password:</b> {driverRequest.UserName+"@123"}</p>
                          <p>Please change your password after first login.</p>
                        "
                   );
               
                var driverUser = _mapper.Map<Driver>(driverRequest);
                driverUser.UserId = userRequest.Id;
                await driverRepository.AddDriver(driverUser);
                return (Guid)userRequest.Id;
            }
            else
            {
                var driverUser = _mapper.Map<Driver>(driverRequest);
                await driverRepository.UpdateDriver(driverUser);
                
                var userRequest = _mapper.Map<User>(driverRequest);
                userRequest.Id = driverRequest.UserId.Value;
                await userRepository.UpdateUser(userRequest);
                return (Guid)driverRequest.Id;
            }
        }
    
        public async Task DeleteDriver(Guid Id)
        {
            await driverRepository.DeleteDriver(Id);
        }

        public async Task<DriverResponse> GetAllDrivers(PagedRequest request)
        {
            return await driverRepository.GetAllDrivers(request);
        }
    }
}
