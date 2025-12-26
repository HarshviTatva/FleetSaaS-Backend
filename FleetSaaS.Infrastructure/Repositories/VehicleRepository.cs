using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class VehicleRepository(
        ApplicationDbContext _dbContext,
        ITenantProvider _tenantProvider
        ) : IVehicleRepository
    {
        public async Task<VehicleResponse> GetAllVehicles(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;

            //fetching all vehicles of particular company
            var vehicles = await (
            from v in _dbContext.Vehicles
            where v.CompanyId == companyId
               && !v.IsDeleted

            select new VehicleDTO
            {
                Id = v.Id,
                Vin = v.Vin,
                LicensePlate = v.LicensePlate,
                Make = v.Make,
                Model = v.Model,
                InsuranceExpiryDate =v.InsuranceExpiryDate,
                Year = v.Year,
                IsActive = v.IsActive
            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                vehicles = vehicles.Where(x =>
                    x.Model.Contains(pagedRequest.Search)).ToList();
            }

            // Sorting
            vehicles = pagedRequest.SortBy switch
            {
                "Model" => pagedRequest.SortDirection == "desc"
                                ? vehicles.OrderByDescending(x => x.Model).ToList()
                                : vehicles.OrderBy(x => x.Model).ToList(),

                _ => vehicles.OrderByDescending(x => x.Id).ToList()
            };

            var totalCount = (vehicles.ToList()).Count();

            vehicles = vehicles.Skip((pagedRequest.PageNumber-1)*pagedRequest.PageSize).Take(pagedRequest.PageSize).ToList();

            return new VehicleResponse
            {
                Vehicles = vehicles,
                CompanyId = companyId,
                TotalCount = totalCount,
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize
            };
        }


        public async Task DeleteVehicle(Guid vehicleId)
        {
            Guid companyId = _tenantProvider.CompanyId;

            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(u => u.Id == vehicleId && !u.IsDeleted && u.CompanyId==companyId);

            if (vehicle != null)
            {
                vehicle.IsActive = false;
                vehicle.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async void AddVehicle(VehicleRequest vehicleRequest)
        {
            await _dbContext.Vehicles.AddAsync(vehicleRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async void UpdateVehicle(VehicleRequest vehicleRequest)
        {
            await _dbContext.Vehicles.Update(vehicleRequest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
