using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController(ICommonService _commonService) : ControllerBase
    {
        //all vehicle assignment dropdown
        [Authorize(Roles = "CompanyOwner, Dispatcher")]
        [HttpGet("assignedDriverVehicles")]
        public async Task<IActionResult> GetAllDriverVehiclesDropdown()
        {
            return Ok(new SuccessApiResponse<object>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await _commonService.GetAllDriverVehiclesDropdown()
                      ));
        }

    }
}
