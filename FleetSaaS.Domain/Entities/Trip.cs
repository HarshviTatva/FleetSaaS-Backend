using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class Trip : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TripStatus Status { get; set; } = TripStatus.Planned;
        public ICollection<TripOdometerLog> OdometerLogs { get; set; }
            = new List<TripOdometerLog>();
    }
}
