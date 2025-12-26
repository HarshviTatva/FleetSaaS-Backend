namespace FleetSaaS.Application.DTOs.Request
{
    public class UserRequest
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? UserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
