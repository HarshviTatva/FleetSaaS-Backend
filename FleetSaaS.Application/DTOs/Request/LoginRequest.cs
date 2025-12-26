using System.ComponentModel.DataAnnotations;

namespace FleetSaaS.Application.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
