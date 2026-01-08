using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;

namespace FleetSaaS.API.Middleware
{
    public class TenantResolutionTestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICompanyService _companyService;
        private readonly ITenantProvider _tenantProvider;

        public TenantResolutionTestMiddleware(RequestDelegate next, ICompanyService companyService,
            ITenantProvider tenantProvider)
        {
            _next = next;
            _companyService = companyService;
            _tenantProvider = tenantProvider;
        }

        public async Task Invoke(HttpContext context)
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

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    //public static class TenantResolutionTestMiddlewareExtensions
    //{
    //    public static IApplicationBuilder UseTenantResolutionTestMiddleware(this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<TenantResolutionTestMiddleware>();
    //    }
    //}
}
