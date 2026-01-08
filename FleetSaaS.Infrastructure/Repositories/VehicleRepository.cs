using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
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
        public async Task<bool> ExistsByVinAsync(string vin, Guid? vehicleId=null)
        {
            return await _dbContext.Vehicles.AnyAsync(x => x.Vin == vin && !x.IsDeleted && x.Id!=vehicleId);
        }

        public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, Guid? vehicleId = null)
        {
            return await _dbContext.Vehicles.AnyAsync(x => x.LicensePlate == licensePlate && !x.IsDeleted && x.Id!=vehicleId);
        }

        public async Task<VehicleResponse> GetAllVehicles(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;

            IQueryable<VehicleDTO> query =
                from v in _dbContext.Vehicles
                where
                !v.IsDeleted
                select new VehicleDTO
                {
                    Id = v.Id,
                    Vin = v.Vin,
                    LicensePlate = v.LicensePlate,
                    Make = v.Make,
                    Model = v.Model,
                    InsuranceExpiryDate = v.InsuranceExpiryDate,
                    Year = v.Year,
                    IsActive = v.IsActive
                };

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();

                query = query.Where(x =>
                    x.Model.ToLower().Contains(search) ||
                    x.Make.ToLower().Contains(search) ||
                    x.LicensePlate.ToLower().Contains(search) ||
                    x.Vin.ToLower().Contains(search)
                );
            }

            if (DateOnly.TryParse(pagedRequest.Date, out var date))
            {
                query = query.Where(v =>
                    v.InsuranceExpiryDate == date
                );
            }


            query = pagedRequest.SortBy switch
            {
                "make" => pagedRequest.SortDirection == "desc"
                                ? query.OrderByDescending(x => x.Make)
                                : query.OrderBy(x => x.Make),
                "vin" => pagedRequest.SortDirection == "desc"
                               ? query.OrderByDescending(x => x.Vin)
                               : query.OrderBy(x => x.Vin),
                "insuranceExpiryDate" => pagedRequest.SortDirection == "desc"
                               ? query.OrderByDescending(x => x.InsuranceExpiryDate)
                               : query.OrderBy(x => x.InsuranceExpiryDate),
                "licensePlate" => pagedRequest.SortDirection == "desc"
                               ? query.OrderByDescending(x => x.LicensePlate)
                               : query.OrderBy(x => x.LicensePlate),
                "model" => pagedRequest.SortDirection == "desc"
                                ? query.OrderByDescending(x => x.Model)
                                : query.OrderBy(x => x.Model),
                "year" => pagedRequest.SortDirection == "desc"
                                ? query.OrderByDescending(x => x.Year)
                                : query.OrderBy(x => x.Year),
                _ => query.OrderByDescending(x => x.Id)
            };

            var totalCount = await query.CountAsync();

            var vehicles = await query
                .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .ToListAsync();

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
                .FirstOrDefaultAsync(u => u.Id == vehicleId && !u.IsDeleted);

            if (vehicle != null)
            {
                vehicle.IsActive = false;
                vehicle.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            vehicle.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.Vehicles.AddAsync(vehicle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            vehicle.CompanyId = _tenantProvider.CompanyId;
            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DropdownResponse>> GetAllVehiclesDropdown()
        {
            return await _dbContext.Vehicles
              .Where(v =>
                  v.IsActive && !v.IsDeleted &&
                  !_dbContext.VehicleAssignments.Any(a =>
                      a.VehicleId == v.Id &&
                      a.IsActive))
              .Select(v => new DropdownResponse
              {
                  Value = v.Id,
                  Label = $"{v.Model} - {v.LicensePlate}"
              })
              .ToListAsync();
        }

        public async Task AssignVehicleToDriver(VehicleAssignment vehicleAssignment)
        {
            vehicleAssignment.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.VehicleAssignments.AddAsync(vehicleAssignment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ReAssignVehicleToDriver(VehicleAssignment vehicleAssignment)
        {
            vehicleAssignment.CompanyId = _tenantProvider.CompanyId;
            VehicleAssignment? assignedVehicleDetails = await _dbContext.VehicleAssignments.FirstOrDefaultAsync(x=>x.Id == vehicleAssignment.Id && !x.IsDeleted && x.IsActive);
            if (assignedVehicleDetails != null)
            {
                assignedVehicleDetails.VehicleId = vehicleAssignment.VehicleId;
                assignedVehicleDetails.UpdatedAt = DateTime.UtcNow;
                assignedVehicleDetails.UpdatedBy = _tenantProvider.CompanyId;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnAssignVehicleToDriver(Guid id)
        {
            VehicleAssignment? assignedVehicleDetails = await _dbContext.VehicleAssignments
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (assignedVehicleDetails != null)
            {
                assignedVehicleDetails.IsActive = false;
                assignedVehicleDetails.IsDeleted = true;
            }
            else
            {
                throw new Exception(message:MessageConstants.NO_RECORD_FOUND);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
