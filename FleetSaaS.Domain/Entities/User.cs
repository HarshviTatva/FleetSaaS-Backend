using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class User : BaseEntity, ITenantEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;
        public int RoleId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
