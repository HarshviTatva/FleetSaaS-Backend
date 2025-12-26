namespace FleetSaaS.Infrastructure.Common
{
    public interface ITenantProvider
    {
        Guid CompanyId { get; set; }
    }
}
