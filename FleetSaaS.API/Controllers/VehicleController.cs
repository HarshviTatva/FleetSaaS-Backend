using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Application.Services;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Helper;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CompanyOwner,Dispatcher,Admin")]
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            await vehicleService.DeleteVehicle(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.DELETED_MESSAGE, "Vehicle") },
            data: id));
        }

        [HttpPost("vehicle")]
        public async Task<IActionResult> AddEditVehicle(VehicleRequest vehicleRequest)
        {
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: StatusCodes.Status201Created,
                message: new List<string> { string.Format(MessageConstants.SAVED_MESSAGE, "Vehicle") },
                data: await vehicleService.AddEditVehicle(vehicleRequest)));
        }

        //vehicle assignment to drivers 
        [HttpGet("all-vehicles")]
        public async Task<IActionResult> GetAllVehiclesDropdown()
        {
            return Ok(new SuccessApiResponse<object>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await vehicleService.GetAllVehiclesDropdown()
                      ));
        }

        [HttpPost("vehicle-assignment")]
        public async Task<IActionResult> AssignVehicleToDriver([FromBody]AssignVehicleRequest assignVehicleRequest)
        {
            return Ok
            (
                new SuccessApiResponse<Guid>
                (
                httpStatusCode: StatusCodes.Status201Created,
                message:new List<string> { string.Format(MessageConstants.ASSIGNED_MESSAGE,"Vehicle") },
                data : await vehicleService.AssignVehicleToDriver(assignVehicleRequest)
                )
            );
        }

        [HttpPatch("vehicle-assignments/{id}")]
        public async Task<IActionResult> ReAssignVehicleToDriver([FromBody]AssignVehicleRequest assignVehicleRequest)
        {
            return Ok
            (
                new SuccessApiResponse<Guid>
                (
                httpStatusCode: StatusCodes.Status200OK,
                message: new List<string> { string.Format(MessageConstants.REASSIGNED_MESSAGE, "Vehicle") },
                data: await vehicleService.ReAssignVehicleToDriver(assignVehicleRequest)
                )
            );
        }

        [HttpPatch("vehicle-unassign/{id}")]
        public async Task<IActionResult> UnAssignVehicleToDriver(Guid id)
        {
            await vehicleService.UnAssignVehicleToDriver(id);
            return Ok
            (
                new SuccessApiResponse<object>
                (
                httpStatusCode: StatusCodes.Status200OK,
                message: new List<string> { string.Format(MessageConstants.UNASSIGNED_MESSAGE, "Vehicle") },
                data: id
                )
            );
        }

        [HttpPost("export/vehicles")]
        public async Task<IActionResult> ExportVehicles([FromBody] PagedRequest pagedRequest)
        {
            var csvBytes = await vehicleService.ExportVehicleToCsvAsync(pagedRequest);
            return File(csvBytes, "text/csv", "vehicles.csv");
        }

    }
}
