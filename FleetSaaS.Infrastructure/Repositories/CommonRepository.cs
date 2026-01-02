using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class CommonRepository(ITenantProvider _tenantProvider, ApplicationDbContext _dbContext) : ICommonRepository
    {
        public async Task<List<DropdownResponse>> GetAllDriverVehiclesDropdown()
        {
            Guid companyId = _tenantProvider.CompanyId;
            return await _dbContext.VehicleAssignments
            .Where(va =>
                va.IsActive &&
                !va.IsDeleted &&
                va.CompanyId == companyId &&
                va.Vehicle.IsActive &&
                !va.Vehicle.IsDeleted &&
                !va.Driver.IsDeleted
            )
            .Select(va => new DropdownResponse
            {
                Value = va.Id,
                Label = $"{va.Vehicle.Model} - {va.Vehicle.LicensePlate} | {va.Driver.User.UserName}"
            })
            .ToListAsync();

        }
    }
}
