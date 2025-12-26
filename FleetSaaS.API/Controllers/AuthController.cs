using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: (int)HttpStatusCode.OK,
                message: new List<string> { MessageConstants.LOGIN_SUCCESS },
                data: await _authService.LoginAsync(request)
            ));
        }

    }
}
