namespace FleetSaaS.Application.DTOs.Response
{
    public class DriverDTO : UserDTO
    {
        public string LicenseNumber { get; set; }
        public DateOnly LicenseExpiryDate { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsVehicleAssigned { get; set; }
        public Guid? VehicleAssignmentId { get; set; }
        public string? VehicleName { get; set; }
    }

    public class DriverResponse() : PagedResponse
    {
        public List<DriverDTO> DriversList { get; set; }
        public Guid CompanyId { get; set; }
        public string Role { get; set; }
    }

    public class UserDTO()
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class DispatcherResponse() :PagedResponse
    {
        public List<UserDTO> DispatcherList { get; set; }
        public Guid CompanyId { get; set; }
        public string Role { get; set; }
    }
}
