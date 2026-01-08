using FleetSaaS.Domain.Entities;
using System.Linq.Expressions;

namespace FleetSaaS.Application.Interfaces.IRepositories.Generic
{
    public interface IGenericRepository<T> where T :  BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        IQueryable<T> Query();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Guid GetCompanyId();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<List<TResult>> FindAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult?> FindFirstAsync<TResult>(
          Expression<Func<T, bool>> predicate,
          Expression<Func<T, TResult>> selector, bool ignoreQuery = false);
    }
}
