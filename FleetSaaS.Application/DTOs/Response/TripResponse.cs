using FleetSaaS.Domain.Enum;

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
        public string Name { get; set; }
        public string Origin { get; set; } 
        public string Destination { get; set; } 
        public string Description { get; set; }
        public TripStatus Status { get; set; }
    }
}
