using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace FleetSaaS.Application.Services
{
    public class CompanyService(
        IUserRepository userRepository,
        IGenericRepository<Company> _companyRepo,
        IGenericRepository<User> _userRepo,
        ICompanyRepository companyRepository, 
        IPasswordHasher<User> _passwordHasher,

        IMapper _mapper) : ICompanyService
    {
        public async Task<string> CompanyRegisterAsync(CompanyUserRegisterRequest companyUserRegisterRequest)
        {
          if (await companyRepository.ExistsByEmailAsync(companyUserRegisterRequest.Email))
                throw new ApplicationException(TenantCommonMessages.ALREADY_REGISTERED);
          
           Company company = _mapper.Map<Company>(companyUserRegisterRequest);

          //await companyRepository.AddAsync(company);
          await _companyRepo.AddAsync(company);

          User ownerUser = _mapper.Map<User>(companyUserRegisterRequest);
          ownerUser.CompanyId = company.Id;
          ownerUser.Password = _passwordHasher.HashPassword(
              ownerUser,
              companyUserRegisterRequest.Password
          );
            ownerUser.RoleId = (int)RoleType.CompanyOwner;
          //await userRepository.AddCompanyUser(ownerUser);
          await _userRepo.AddAsync(ownerUser);
          return ownerUser.CompanyId.ToString();
        }
        
        public async Task<bool> IsCompanyRegistered(Guid companyId)
        {
          return await companyRepository.CheckCompanyExists(companyId);
        }
    }
}
