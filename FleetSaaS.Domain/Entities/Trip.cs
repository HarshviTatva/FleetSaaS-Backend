using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class Trip : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }

        //public Guid VehicleId { get; set; }
        //public Vehicle Vehicle { get; set; } = null!;

        //public Guid DriverId { get; set; }
        //public Driver Driver { get; set; } = null!;

        public Guid VehicleAssignmentId { get; set; }
        public VehicleAssignment VehicleAssignment { get; set; } = null!;

        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;

        public DateTime PlannedStartTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }

        public TripStatus Status { get; set; } = TripStatus.Planned;
        public ICollection<TripOdometerLog> OdometerLogs { get; set; }
            = new List<TripOdometerLog>();
    }
}
