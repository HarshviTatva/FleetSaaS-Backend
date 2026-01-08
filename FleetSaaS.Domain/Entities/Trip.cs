using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Interface;
using System.ComponentModel.DataAnnotations;

namespace FleetSaaS.Domain.Entities
{
    public class Trip : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public DateTime? ScheduledAt { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TripStatus Status { get; set; } = TripStatus.Planned;
        [MaxLength(100)]
        public string? CancelReason { get; set; }
        public Guid? VehicleAssignmentId { get; set; }
        public VehicleAssignment VehicleAssignment { get; set; }
        public long? DistanceCovered { get; set; }
    }
}
