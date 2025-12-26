using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class DispatcherRepository(
        ApplicationDbContext _dbContext,
        ITenantProvider _tenantProvider
        ) : IDispatcherRepository
    {
        public async Task<DispatcherResponse> GetAllDispatchers(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;

            //fetching all dispatchers of particular company
            var dispatchers = await (
            from u in _dbContext.Users
            where u.CompanyId == companyId
               && !u.IsDeleted
               && u.RoleId == (int)RoleType.Dispatcher

            select new UserDTO
            {
                Id = u.Id,  
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                dispatchers = dispatchers.Where(x =>
                    x.UserName.Contains(pagedRequest.Search) ||
                    x.Email.Contains(pagedRequest.Search) ||
                    x.PhoneNumber.Contains(pagedRequest.Search)).ToList();
            }

            // Sorting
            dispatchers = pagedRequest.SortBy switch
            {
                "UserName" => pagedRequest.SortDirection == "desc"
                                ? dispatchers.OrderByDescending(x => x.UserName).ToList()
                                : dispatchers.OrderBy(x => x.UserName).ToList(),

                _ => dispatchers.OrderByDescending(x => x.Id).ToList()
            };

            var totalCount = (dispatchers.ToList()).Count();

            dispatchers = dispatchers.Skip((pagedRequest.PageNumber-1)*pagedRequest.PageSize).Take(pagedRequest.PageSize).ToList();

            return new DispatcherResponse
            {
                DispatcherList = dispatchers,
                CompanyId = companyId,
                Role = RoleType.Driver.ToString(),
                TotalCount = totalCount,
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize
            };

        }

        public async Task DeleteDispatcher(Guid dispatcherId)
        {
            Guid companyId = _tenantProvider.CompanyId;

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == dispatcherId && !u.IsDeleted && u.CompanyId==companyId);

            if (user != null)
            {
                user.IsActive = false;
                user.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
