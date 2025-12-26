using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class Driver : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public string LicenseNumber { get; set; }
        public DateOnly? LicenseExpiry {  get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<VehicleAssignment> VehicleAssignments { get; set; }
            = new List<VehicleAssignment>();

        public ICollection<Trip> Trips { get; set; }
            = new List<Trip>();
    }
}
