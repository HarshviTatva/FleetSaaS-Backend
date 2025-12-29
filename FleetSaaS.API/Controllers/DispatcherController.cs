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
    [Authorize(Roles = "CompanyOwner")]
    public class DispatcherController(IDispatcherService dispatcherService) : ControllerBase
    {
        [HttpGet("dispatchers")]
        public async Task<IActionResult> GetAllDispatchers([FromQuery] PagedRequest request)
        {
            return Ok(new SuccessApiResponse<object>(
                      httpStatusCode: StatusCodes.Status201Created,
                      message: new List<string> { MessageConstants.DATA_RETRIEVED },
                      data: await dispatcherService.GetAllDispatchers(request)
                      ));
        }

        [HttpPost("dispatcher")]
        public async Task<IActionResult> AddEditDispatcherUser(UserRequest userRequest)
        {
            bool isUpdate = userRequest.Id.HasValue;
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: isUpdate
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status201Created,
                message: new List<string>
                {
                 string.Format(isUpdate? MessageConstants.UPDATED_MESSAGE: MessageConstants.CREATED_MESSAGE,"Dispatcher")
                },
                data: await dispatcherService.AddEditDispatcher(userRequest)
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDispatcher(Guid id)
        {
            await dispatcherService.DeleteDispatcher(id);
            return Ok(new SuccessApiResponse<object>(
            httpStatusCode: StatusCodes.Status201Created,
            message: new List<string> { string.Format(MessageConstants.DELETED_MESSAGE, "Dispatcher") },
            data: null));
        }
    }
}
