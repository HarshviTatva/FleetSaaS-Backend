using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IDashboardService _dashboardService,ITenantProvider _tenantProvider) : ControllerBase
    {
        [Authorize(Roles = "CompanyOwner")]
        [HttpGet("company-user-dashboard")] 
        public async Task<IActionResult> GetCompanyOwnerDashboardDetails()
        {
            Guid companyId = _tenantProvider.CompanyId;
            return Ok(new SuccessApiResponse<object>(
                   httpStatusCode: StatusCodes.Status201Created,
                   message: new List<string> { MessageConstants.DATA_RETRIEVED },
                   data: await _dashboardService.GetCompanyUserDashboardDetails(companyId)
                   ));
        }

        [Authorize(Roles = "Dispatcher")]
        [HttpGet("dispatcher-dashboard")]
        public async Task<IActionResult> GetDispatcherDashboardDetails()
        {
            Guid companyId = _tenantProvider.CompanyId;
            return Ok(new SuccessApiResponse<object>(
                   httpStatusCode: StatusCodes.Status201Created,
                   message: new List<string> { MessageConstants.DATA_RETRIEVED },
                   data: await _dashboardService.GetDispatcherDashboardDetails(companyId)
                   ));
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("driver-dashboard")]
        public async Task<IActionResult> GetDriverDashboardDetails()
        {
            Guid companyId = _tenantProvider.CompanyId;
            return Ok(new SuccessApiResponse<object>(
                   httpStatusCode: StatusCodes.Status201Created,
                   message: new List<string> { MessageConstants.DATA_RETRIEVED },
                   data: await _dashboardService.GetDriverDashboardDetails(companyId)
                   ));
        }
    }
}
