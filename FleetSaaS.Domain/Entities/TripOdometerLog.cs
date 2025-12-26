using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class TripOdometerLog : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = default!;
        public int OdometerValue { get; set; }
        public string LogType { get; set; }
        public DateTime LoggedAt { get; set; }
    }
}
