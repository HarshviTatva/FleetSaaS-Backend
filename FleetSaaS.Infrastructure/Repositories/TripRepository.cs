using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class TripRepository(ITenantProvider _tenantProvider,ApplicationDbContext _dbContext) : ITripRepository
    {
        public async Task<TripResponse> GetAllTrips(PagedRequest pagedRequest)
        {
            Guid companyId = _tenantProvider.CompanyId;
            var trips = _dbContext.Trips.AsNoTracking().Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();

                trips = trips.Where(t =>
                    t.Origin.ToLower().Contains(search) ||
                    t.Destination.ToLower().Contains(search)
                );
            }

            if (pagedRequest.Status>0)
            {
                trips = trips.Where(t =>t.Status == (TripStatus)pagedRequest.Status);
            }

            trips = pagedRequest.SortBy?.ToLower() switch
            {

                "status" => pagedRequest.SortDirection == "asc"
                    ? trips.OrderBy(t => t.Status)
                    : trips.OrderByDescending(t => t.Status),

                _ => trips.OrderByDescending(t => t.Id)
            };

            var totalCount = await trips.CountAsync();

            var response = await trips
                .Select(t => new TripDTO
                {
                    Id = t.Id,
                    Name = GenerateTripName(t.Origin,t.Destination,t.CreatedAt),
                    Origin = t.Origin,
                    Destination = t.Destination,
                    Description = t.Description,
                    ScheduledAt = t.ScheduledAt != null? t.ScheduledAt.Value.ToString(): null,
                    Status = t.Status      
                })
                .ToListAsync();

            trips = trips.Skip((pagedRequest.PageNumber-1)*pagedRequest.PageSize).Take(pagedRequest.PageSize);

            return new TripResponse
            {
                Trips = response,
                TotalCount = totalCount,
                CompanyId = companyId,
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize
            };
        }

        public static string GenerateTripName(string origin, string destination, DateTime? plannedDate)
        {
            string from = string.IsNullOrWhiteSpace(origin)
            ? string.Empty
            : origin.Trim()[..Math.Min(3, origin.Trim().Length)].ToUpper();

            string to = string.IsNullOrWhiteSpace(destination)
                ? string.Empty
                : destination.Trim()[..Math.Min(3, destination.Trim().Length)].ToUpper();

            string datePart = plannedDate.HasValue
                ? plannedDate.Value.Day.ToString("D3") 
                : "000";

            return $"{from}TO{to}{datePart}";
        }

        public async Task AddTrip(Trip trip)
        {
            trip.CompanyId = _tenantProvider.CompanyId;
            await _dbContext.Trips.AddAsync(trip);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTrip(Trip trip)
        {
            trip.CompanyId = _tenantProvider.CompanyId;
            _dbContext.Trips.Update(trip);
            await _dbContext.SaveChangesAsync();
        }
    }
}
