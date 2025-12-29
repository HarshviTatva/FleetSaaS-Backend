using AutoMapper;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class UserRepository(
        ApplicationDbContext _dbContext,
        ITenantProvider _tenantProvider
        ) : IUserRepository
     {
        public async Task<bool> ExistsByEmailAsync(string email,Guid? userId = null)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email && !x.IsDeleted && x.Id!=userId);
        }

        public async Task AddCompanyUser(User user)
        {
            if (user?.CompanyId == null)
                throw new InvalidOperationException(TenantCommonMessages.INFO_NOT_FOUND);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddTenantAsUser(User user)
        {
            if (_tenantProvider?.CompanyId == null ||  _tenantProvider.CompanyId == Guid.Empty)
                throw new InvalidOperationException(TenantCommonMessages.INFO_NOT_FOUND);
            user.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<User?> GetActiveUserWithRolesByEmailAsync(string email)
        {
            return await _dbContext.Users
                .IgnoreQueryFilters()
                .AsNoTracking()
                //.Include(u => u.UserRoles)
                //    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task UpdateUser(User user)
        {
            var companyId = _tenantProvider.CompanyId;
            var userData = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id && u.CompanyId == companyId);

            if (userData == null || user == null)
                throw new UnauthorizedAccessException(MessageConstants.DATA_RETRIEVAL_FAILED);

            userData.UserName = user.UserName;
            userData.PhoneNumber = user.PhoneNumber;
            userData.Email = user.Email;
            userData.IsActive = user.IsActive;
            await _dbContext.SaveChangesAsync();
        }

    }
}
