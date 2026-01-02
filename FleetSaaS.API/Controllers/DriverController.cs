using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
    public class DriverController(IDriverService _driverService, ITripService _tripService) : ControllerBase
    {
        [Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDriver([FromQuery] PagedRequest request)
        {
          return Ok(new SuccessApiResponse<object>(
                    httpStatusCode: StatusCodes.Status201Created,
                    message: new List<string> { MessageConstants.DATA_RETRIEVED },
                    data: await _driverService.GetAllDrivers(request)
                    ));
        }

        [Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
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

        [Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        { 
            await _driverService.DeleteDriver(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.DELETED_MESSAGE, "Driver") },
            data: null));
        }

        //driver can view only assigned vehicles
        [Authorize(Roles = "Driver")]
        [HttpGet("assigned-vehicle")]
        public async Task<IActionResult> GetAssignedVehicle()
        {
            return Ok(new SuccessApiResponse<object>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await _driverService.GetAssignedVehicle()
                      ));
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("assigned-trips")]
        public async Task<IActionResult> GetAllAssignedTrips([FromQuery] PagedRequest pagedRequest)
        {
            return Ok(new SuccessApiResponse<TripResponse>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await _tripService.GetAllTrips(pagedRequest)
                      ));
        }
    }
}
