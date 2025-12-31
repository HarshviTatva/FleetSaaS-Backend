using System.ComponentModel.DataAnnotations;

namespace FleetSaaS.Application.DTOs.Request
{
    public class CompanyUserRegisterRequest
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string OwnerEmail { get; set; }

        [Required]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
