using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(Company company);
        Task<bool> CheckCompanyExists(Guid companyId);
    }
}
