using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Exceptions;
using FleetSaaS.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace FleetSaaS.Application.Services
{
    public class DriverService(
        IUserRepository _userRepository,
        IEmailService _emailService,
        IPasswordHasher<User> _passwordHasher,
        IMapper _mapper,
        IDriverRepository _driverRepository,
        ICommonService _commonService
        ) : IDriverService
    {
        public async Task<Guid> AddEditDriver(DriverUserRequest driverRequest)
        {
            if (await _userRepository.ExistsByEmailAsync(driverRequest.Email, driverRequest.Id))
                throw new ConflictException(field: Fields.Email, message: MessageConstants.USER_EXISTS);

            if (await _driverRepository.ExistsByLicenseNumberAsync(driverRequest.LicenseNumber, driverRequest.Id))
                throw new ConflictException(field: Fields.License_Number, message: DriverMessages.LICENSE_NO_EXISTS);

            Driver driverUser = _mapper.Map<Driver>(driverRequest);
            User userRequest = _mapper.Map<User>(driverRequest);

            if (driverRequest?.Id==null)
            {
                string randomPassword = _commonService.GenerateRandomPassword(8);
                userRequest.Password = _passwordHasher.HashPassword(
                      userRequest,
                      randomPassword
                  );
                userRequest.RoleId = (int)RoleType.Driver;
                await _userRepository.AddTenantAsUser(userRequest);

                //email by smtp
                await _emailService.SendAsync(
                       driverRequest.Email,
                       "Your Driver Account Created",
                       $@"
                          <h3>Welcome {driverRequest.UserName}</h3>
                          <p>Your driver account has been created.</p>
                          <p><b>Username:</b> {driverRequest.UserName}</p>
                          <p><b>Password:</b> {randomPassword}</p>
                          <p>Please change your password after first login.</p>
                        "
                   );
               
                driverUser.UserId = userRequest.Id;
                await _driverRepository.AddDriver(driverUser);
                return userRequest.Id;
            }
            else
            {
                await _driverRepository.UpdateDriver(driverUser);
                userRequest.Id = driverRequest.UserId.Value;
                await _userRepository.UpdateUser(userRequest);
                return (Guid)driverRequest.Id;
            }
        }
    
        public async Task DeleteDriver(Guid Id)
        {
            await _driverRepository.DeleteDriver(Id);
        }

        public async Task<DriverResponse> GetAllDrivers(PagedRequest request)
        {
            return await _driverRepository.GetAllDrivers(request);
        }

        public async Task<VehicleDTO> GetAssignedVehicle()
        {
            return await _driverRepository.GetAssignedVehicle();
        }

    }
}
