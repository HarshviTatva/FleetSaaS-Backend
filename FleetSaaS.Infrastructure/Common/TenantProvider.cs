using Microsoft.AspNetCore.Http;

namespace FleetSaaS.Infrastructure.Common
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CompanyId
        {
            get
            {
                var companyIdClaim =
                    _httpContextAccessor.HttpContext?.User?
                        .FindFirst("CompanyId")?.Value;

                return companyIdClaim is null
                    ? Guid.Empty
                    : Guid.Parse(companyIdClaim);
            }

            set
            {

            }
        }
    }
}
