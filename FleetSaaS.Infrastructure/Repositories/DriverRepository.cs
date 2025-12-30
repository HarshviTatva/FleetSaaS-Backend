using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class DriverRepository
        (ApplicationDbContext _dbContext,
        ITenantProvider _tenantProvider) : IDriverRepository
       {
        public async Task<bool> ExistsByLicenseNumberAsync(string licenseNumber, Guid? driverId = null)
        {
            return await _dbContext.Drivers.AnyAsync(x => x.LicenseNumber == licenseNumber && !x.IsDeleted && x.Id!=driverId);
        }

        public async Task<DriverResponse> GetAllDrivers(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;

            //fetching all drivers of particular company
            var drivers = await (
            from d in _dbContext.Drivers
            join u in _dbContext.Users on d.UserId equals u.Id
            join va in _dbContext.VehicleAssignments
            .Where(x => x.IsActive && !x.IsDeleted)
            on d.Id equals va.DriverId into vaGroup
            from va in vaGroup.DefaultIfEmpty()
            join v in _dbContext.Vehicles
            .Where(x=>x.IsActive && !x.IsDeleted)
            on va.VehicleId equals v.Id into vGroup
            from v in vGroup.DefaultIfEmpty()
            where d.CompanyId == companyId
               && !d.IsDeleted
               && !u.IsDeleted
               && u.RoleId == (int)RoleType.Driver

            select new DriverDTO
            {
                    Id = d.Id,
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    LicenseNumber = d.LicenseNumber,
                    LicenseExpiryDate = (DateOnly)(d.LicenseExpiry),
                    IsAvailable = d.IsAvailable,
                    IsActive = u.IsActive,
                    IsVehicleAssigned = va != null,
                    VehicleAssignmentId = va != null ? va.Id : null,
                    VehicleName = $"{v.Model} - {v.LicensePlate}"
            }).ToListAsync();

            // Sorting
            drivers = pagedRequest.SortBy switch
            {
                "UserName" => pagedRequest.SortDirection == "desc"
                                ? drivers.OrderByDescending(x => x.UserName).ToList()
                                : drivers.OrderBy(x => x.UserName).ToList(),

                _ => drivers.OrderByDescending(x => x.Id).ToList()
            };

            var totalCount = (drivers.ToList()).Count();

            drivers = drivers.Skip((pagedRequest.PageNumber-1)*pagedRequest.PageSize).Take(pagedRequest.PageSize).ToList();

            return new DriverResponse
            {
                DriversList = drivers,
                CompanyId = companyId,
                Role = RoleType.Driver.ToString(),
                TotalCount = totalCount,
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize
            };

        }

        public async Task AddDriver(Driver driver)
        {
            driver.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.Drivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDriver(Driver driverUser)
        {
            Guid companyId = _tenantProvider.CompanyId;
            var driverUserData = await _dbContext.Drivers
                .FirstOrDefaultAsync(u => u.Id == driverUser.Id && u.CompanyId == companyId);

            if (driverUserData == null)
                throw new UnauthorizedAccessException(MessageConstants.DATA_RETRIEVAL_FAILED);

            driverUserData.LicenseExpiry = driverUser.LicenseExpiry;
            driverUserData.LicenseNumber =driverUser.LicenseNumber;
            driverUserData.IsAvailable = driverUser.IsAvailable;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteDriver(Guid driverId)
        {
            Guid companyId = _tenantProvider.CompanyId;

            var driver = await _dbContext.Drivers
                .FirstOrDefaultAsync(d =>
                    d.Id == driverId &&
                    d.CompanyId == companyId &&
                    !d.IsDeleted);

            if (driver == null)
                throw new UnauthorizedAccessException("Driver not found");

            driver.IsDeleted = true;

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == driver.UserId && !u.IsDeleted);

            if (user != null)
            {
                user.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
