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
        ApplicationDbContext _dbContext
        ) : IUserRepository
     {
        public async Task<bool> ExistsByEmailAsync(string email,Guid? userId = null)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email && !x.IsDeleted && x.Id!=userId);
        }

        public async Task<User?> GetActiveUserWithRolesByEmailAsync(string email)
        {
            return await _dbContext.Users
                .IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task UpdateUser(User user)
        {
            var userData = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

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
