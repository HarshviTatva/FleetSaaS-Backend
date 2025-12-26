using FleetSaaS.Domain.Interface;

namespace FleetSaaS.Domain.Entities
{
    public class AuditLogs : BaseEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public AuditLogs Action{ get; set; }//why like this?
        public string EntityName { get; set; } = default!;
        public Guid EntityId { get; set; }

    }
}
