using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class CompanyRepository(ApplicationDbContext _dbContext) : ICompanyRepository
    {
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Companies.AnyAsync(x => x.Email == email);
        }

        public async Task AddAsync(Company company)
        {
            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckCompanyExists(Guid companyId)
        {
            return await _dbContext.Companies
                .AsNoTracking()
                .AnyAsync(c =>
                    c.Id == companyId &&
                    c.IsActive &&
                    !c.IsDeleted
                );
        }
    }
}
