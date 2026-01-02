using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Application.Services;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(ITripService _tripService) : ControllerBase
    {
        //trips
        [HttpGet("trips")]
        public async Task<IActionResult> GetAllTrips([FromQuery] PagedRequest pagedRequest)
        {
            return Ok(new SuccessApiResponse<object>(
                    httpStatusCode: StatusCodes.Status201Created,
                    message: new List<string> { MessageConstants.DATA_RETRIEVED },
                    data: await _tripService.GetAllTrips(pagedRequest)
                    ));
        }

        [HttpPost("trip")]
        public async Task<IActionResult> AddEditTrip(TripRequest tripRequest)
        {
            bool isUpdate = tripRequest.Id.HasValue;
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: isUpdate
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status201Created,
                message: new List<string>
                {
                 string.Format(isUpdate? MessageConstants.UPDATED_MESSAGE: MessageConstants.CREATED_MESSAGE,"Trip")
                },
                data: await _tripService.AddEditTrip(tripRequest)
            ));
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> CancelTrip(Guid id)
        {
            await _tripService.CancelTrip(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.CANCELLED_MESSAGE, "Trip") },
            data: id));
        }

        //assign, reassign, unassign trips to driver
        [HttpPost("assign-driver")]
        public async Task<IActionResult> AssignTripToDriver([FromBody] AssignTripDriverRequest assignTripRequest)
        {
            await _tripService.AssignTripToDriver(assignTripRequest);
            return Ok
            (
                new SuccessApiResponse<object>
                (
                httpStatusCode: StatusCodes.Status201Created,
                message: new List<string> { string.Format(MessageConstants.ASSIGNED_MESSAGE, "Trip") },
                data:null
                )
            );
        }

        [HttpPatch("unassign-driver/{tripId}")]
        public async Task<IActionResult> UnAssignTripToDriver(Guid tripId)
        {
            await _tripService.UnAssignTripToDriver(tripId);
            return Ok
            (
                new SuccessApiResponse<object>
                (
                httpStatusCode: StatusCodes.Status201Created,
                message: new List<string> { string.Format(MessageConstants.UNASSIGNED_MESSAGE, "Trip") },
                data: null
                )
            );
        }
    }
}
