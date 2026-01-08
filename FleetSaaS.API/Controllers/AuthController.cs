using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok();
        }

        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _authService.ForgotPasswordAsync(email);
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: (int)HttpStatusCode.OK,
                message: new List<string> { MessageConstants.RESET_PASSWORD_MAIL },
                data: null
            ));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            await _authService.ResetPasswordAsync(resetPasswordRequest);
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: (int)HttpStatusCode.OK,
                message: new List<string> { MessageConstants.RESET_PASSWORD_SUCCESS },
                data: null
            ));
        }
    }
}
