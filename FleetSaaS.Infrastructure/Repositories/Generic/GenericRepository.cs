using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FleetSaaS.Infrastructure.Repositories.Generic
{
    public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        private readonly ITenantProvider _tenantProvider;

        public GenericRepository(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _dbContext = context;
            _dbSet = context.Set<T>(); 
            _tenantProvider = tenantProvider;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = GetCompanyId();
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = GetCompanyId();
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new Exception($"{typeof(T).Name} not found");

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public Guid GetCompanyId()
        {
            return _tenantProvider.CompanyId;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .CountAsync(predicate);
        }

        public async Task<List<TResult>> FindAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector)
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<TResult?> FindFirstAsync<TResult>(Expression<Func<T, bool>> predicate,Expression<Func<T, TResult>> selector,bool ignoreQuery=false)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();
            if (ignoreQuery)
            {
                query = query.IgnoreQueryFilters();
            }
            return await query.Where(predicate).Select(selector).FirstOrDefaultAsync();
        }

    }
}
