using FleetSaaS.Domain.Enum;
using FleetSaaS.Domain.Helper;
using FleetSaaS.Infrastructure.Common;

namespace FleetSaaS.Application.DTOs.Response
{
    public class TripResponse : PagedResponse
    {
        public List<TripDTO> Trips { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class TripDTO
    {
        public Guid Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TripStatus Status { get; set; }
        public string? ScheduledAt {  get; set; }
        public string? VehicleDriverName { get; set; }
        public Guid? VehicleAssignmentId { get; set; }
        public Guid? DriverUserId { get; set; }
        public long? DistanceCovered { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? StatusName { get; set; }
        public DateTime? ScheduleDateFilter { get; set; }
    }

    public class TripExportDTO 
    {
        public string TripCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string VehicleDriverName { get; set; }
        public string ScheduledAt { get; set; } 
        public string StatusName { get; set; }
        public TripStatus Status { get; set; }
        public long? DistanceCovered { get; set; }
    }

    public class TripPdfDTO : TripExportDTO
    {
        public string? VehicleNumber { get; set; }
        public string? DriverName    { get; set; }
        public string Description { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Completed_At { get; set; }
        public string? CancelReason { get; set; }
    }

}
