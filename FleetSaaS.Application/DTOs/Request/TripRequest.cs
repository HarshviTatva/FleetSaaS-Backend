using System.ComponentModel.DataAnnotations;

namespace FleetSaaS.Application.DTOs.Request
{
    public class TripRequest
    {
        public Guid? Id { get; set; }

        [Required]
        public string Origin { get; set; }
        
        [Required]
        public string Destination { get; set; }
        public string? Description { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}
