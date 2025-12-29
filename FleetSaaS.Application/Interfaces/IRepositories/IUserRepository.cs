using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email, Guid? userId = null);
        Task AddCompanyUser(User user);
        Task<User?> GetActiveUserWithRolesByEmailAsync(string email);
        Task UpdateUser(User user);

        Task AddTenantAsUser(User user);
    }
}
