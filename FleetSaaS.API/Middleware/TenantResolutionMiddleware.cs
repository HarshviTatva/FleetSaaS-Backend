using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;

namespace FleetSaaS.API.Middleware
{
    public class TenantResolutionMiddleware : IMiddleware
    {
        private readonly ICompanyService _companyService;
        private readonly ITenantProvider _tenantProvider;

        public TenantResolutionMiddleware(
            ICompanyService companyService,
            ITenantProvider tenantProvider)
        {
            _companyService = companyService;
            _tenantProvider = tenantProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var tenantClaim = context.User.FindFirst("CompanyId");

                if (tenantClaim == null ||
                    !Guid.TryParse(tenantClaim.Value, out var companyId))
                {
                    throw new UnauthorizedAccessException(TenantCommonMessages.INFO_NOT_FOUND);
                }

                var isCompanyRegistered =
                    await _companyService.IsCompanyRegistered(companyId);

                if (!isCompanyRegistered)
                {
                    throw new UnauthorizedAccessException(TenantCommonMessages.INACTIVE_COMPANY);
                }

                _tenantProvider.CompanyId = companyId;
            }

            await next(context);
        }
    }
}
