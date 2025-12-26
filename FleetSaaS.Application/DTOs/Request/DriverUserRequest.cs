namespace FleetSaaS.Application.DTOs.Request
{
    public class DriverUserRequest : UserRequest
    {
        public string LicenseNumber { get; set; }
        public DateOnly? LicenseExpiryDate { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
