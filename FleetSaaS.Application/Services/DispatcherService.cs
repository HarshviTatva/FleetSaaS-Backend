using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace FleetSaaS.Application.Services
{
    public class DispatcherService(
        IDispatcherRepository _dispatcherRepository,
        IUserRepository _userRepository,
        IGenericRepository<User> _genericRepository,
        IEmailService _emailService,
        ICommonService _commonService,
        IPasswordHasher<User> _passwordHasher,
         
        IMapper _mapper) : IDispatcherService
    {
        public async Task<DispatcherResponse> GetAllDispatchers(PagedRequest request)
        {
            return await _dispatcherRepository.GetAllDispatchers(request);
        }

        public async Task<Guid> AddEditDispatcher(UserRequest userRequest)
        {
            User dispatcherRequest = _mapper.Map<User>(userRequest);
            if (userRequest?.Id==null)
            {
                if (await _userRepository.ExistsByEmailAsync(userRequest.Email,userRequest.Id))
                    throw new ApplicationException(MessageConstants.USER_EXISTS);

                string randomPassword = _commonService.GenerateRandomPassword(8);

                dispatcherRequest.Password = _passwordHasher.HashPassword(
                      dispatcherRequest,
                      randomPassword
                  );
                dispatcherRequest.RoleId = (int)RoleType.Dispatcher;
                dispatcherRequest.CompanyId = _genericRepository.GetCompanyId();
                await _genericRepository.AddAsync(dispatcherRequest);
                //await _userRepository.AddTenantAsUser(dispatcherRequest);

                //email by smtp

                string loginUrl = $"http://localhost:4200/login";

                await _emailService.SendAsync(
                       userRequest.Email,
                       "Your Dispatcher Account Created",
                       $@"
                          <h3>Welcome {userRequest.UserName}</h3>
                          <p>Your dispatcher account has been created.</p>
                          <p><b>Username:</b> {userRequest.UserName}</p>
                          <p><b>Password:</b> {randomPassword}</p>
                          <p>Please change your password after first login.</p>
                          <a href='{loginUrl}'
                             style='display:inline-block;
                                    padding:12px 20px;
                                    background-color:#ef5350;
                                    color:#ffffff;
                                    text-decoration:none;
                                    font-size:16px;
                                    font-weight:600;
                                    border-radius:6px;
                                    font-family:Arial, sans-serif;
                                    '>
                              Login
                          </a>
                        "
                   );    
            }
            else
            {
                userRequest.Id = dispatcherRequest.Id;
                await _userRepository.UpdateUser(dispatcherRequest);
            }
            return dispatcherRequest.Id;
        }

        public async Task DeleteDispatcher(Guid Id)
        {
            await _dispatcherRepository.DeleteDispatcher(Id);
        }

    }
}
