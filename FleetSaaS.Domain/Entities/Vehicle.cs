using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class Vehicle :BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public string Make { get; set; } = default!;
        public string Model { get; set; } = default!;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = default!;
        public string Vin { get; set; } = default!;
        public DateOnly? InsuranceExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<VehicleAssignment> Assignments { get; set; }
            = new List<VehicleAssignment>();
        
    }
}
