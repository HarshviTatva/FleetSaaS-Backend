namespace FleetSaaS.Application.DTOs.Request
{
    public class CompanyUserRegisterRequest
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
