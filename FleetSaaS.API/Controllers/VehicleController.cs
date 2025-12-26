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
    public class VehicleController(IVehicleService vehicleService) : ControllerBase
    {
        //add, edit, delete vehicle records
        [HttpGet("vehicles")]
        public async Task<IActionResult> GetAllVehicles([FromQuery] PagedRequest request)
        {
            return Ok(new SuccessApiResponse<object>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await vehicleService.GetAllVehicles(request)
                      ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            await vehicleService.DeleteVehicle(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.DELETED_MESSAGE, "Vehicle") },
            data: null));
        }

        [HttpPost("vehicle")]
        public async Task<IActionResult> AddEditVehicle(VehicleRequest vehicleRequest)
        {
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: StatusCodes.Status201Created,
                message: new List<string> { string.Format(MessageConstants.SAVED_MESSAGE, "Vehicle") },
                data: await vehicleService.AddEditVehicle(vehicleRequest)));
        }

        //automated insurance expiry date alerts
        //vehicle assignment to drivers 
        //driver can view only assigned vehicles


    }
}
