using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class VehicleAssignment : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = default!;
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; } = default!;
        public DateTime? AssignedFrom {  get; set; }
        public DateTime? AssignedTo { get; set; }
    }
}
