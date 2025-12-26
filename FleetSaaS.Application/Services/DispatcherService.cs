using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace FleetSaaS.Application.Services
{
    public class DispatcherService(
        IDispatcherRepository dispatcherRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordHasher<User> _passwordHasher,
        IMapper _mapper) : IDispatcherService
    {
        public async Task<DispatcherResponse> GetAllDispatchers(PagedRequest request)
        {
            return await dispatcherRepository.GetAllDispatchers(request);
        }

        public async Task<Guid> AddEditDispatcher(UserRequest userRequest)
        {
            var dispatcherRequest = _mapper.Map<User>(userRequest);
            if (userRequest?.Id==null)
            {
                if (await userRepository.ExistsByEmailAsync(userRequest.Email))
                    throw new ApplicationException(MessageConstants.USER_EXISTS);

               
                dispatcherRequest.Password = _passwordHasher.HashPassword(
                      dispatcherRequest,
                      userRequest.UserName+"@123"
                  );
                dispatcherRequest.RoleId = (int)RoleType.Dispatcher;
                await userRepository.AddTenantAsUser(dispatcherRequest);

                //email by smtp
                await emailService.SendAsync(
                       userRequest.Email,
                       "Your Dispatcher Account Created",
                       $@"
                          <h3>Welcome {userRequest.UserName}</h3>
                          <p>Your dispatcher account has been created.</p>
                          <p><b>Username:</b> {userRequest.UserName}</p>
                          <p><b>Password:</b> {userRequest.UserName+"@123"}</p>
                          <p>Please change your password after first login.</p>
                        "
                   );
          
            }
            else
            {
                userRequest.Id = dispatcherRequest.Id;
                await userRepository.UpdateUser(dispatcherRequest);
            }
            return (Guid)dispatcherRequest.Id;
        }

        public async Task DeleteDispatcher(Guid Id)
        {
            await dispatcherRepository.DeleteDispatcher(Id);
        }

    }
}
