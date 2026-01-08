using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync();
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordRequest request);
    }
}
