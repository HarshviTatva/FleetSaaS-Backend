using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
    public class DriverController(IDriverService _driverService) : ControllerBase
    {
        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDriver([FromQuery] PagedRequest request)
        {
          return Ok(new SuccessApiResponse<object>(
                    httpStatusCode: StatusCodes.Status201Created,
                    message: new List<string> { MessageConstants.DATA_RETRIEVED },
                    data: await _driverService.GetAllDrivers(request)
                    ));
        }

        [HttpPost("driver")]
        public async Task<IActionResult> AddEditDriverUser(DriverUserRequest driverRequest)
        {
            bool isUpdate = driverRequest.Id.HasValue;
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: isUpdate
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status201Created,
                message: new List<string>
                {
                 string.Format(isUpdate? MessageConstants.UPDATED_MESSAGE: MessageConstants.CREATED_MESSAGE,"Driver")
                },
                data: await _driverService.AddEditDriver(driverRequest)
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        { 
            await _driverService.DeleteDriver(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.DELETED_MESSAGE, "Driver") },
            data: null));
        }
    }
}
