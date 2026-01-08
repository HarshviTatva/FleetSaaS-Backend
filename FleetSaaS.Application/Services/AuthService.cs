using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FleetSaaS.Application.Services
{
    public class AuthService(
        IUserRepository _userRepository,
        IPasswordHasher<User> _passwordHasher,
        IGenericRepository<User> _userGenericRepository,
        IHttpContextAccessor _httpContextAccessor,
        IEmailService _emailService,
        IConfiguration _configuration) : IAuthService
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetActiveUserWithRolesByEmailAsync(request.Email);

            if (user == null)
                throw new Exception(MessageConstants.INVALID_USER_DETAILS);

            var result = _passwordHasher.VerifyHashedPassword(user,user.Password,request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException(MessageConstants.INVALID_CREDENTIALS);

            var token = GenerateToken(user);

            var refreshToken = GenerateToken(user);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userGenericRepository.UpdateAsync(user);

            return new LoginResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }

        private string GenerateToken(User user)
        {
            RoleType role = (RoleType)user.RoleId;
            var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("CompanyId", user.CompanyId.ToString()),
                    new Claim("RoleId",user.RoleId.ToString()),
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim("Username",user.UserName),
                    new Claim (ClaimTypes.Email, user.Email)
                };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _userGenericRepository.FindFirstAsync<User>(u=>u.RefreshToken== request.RefreshToken,u=>u,true);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException(MessageConstants.INVALID_TOKEN);

            var newAccessToken = GenerateToken(user);
            var newRefreshToken = GenerateToken(user);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userGenericRepository.UpdateAsync(user);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }

        public async Task LogoutAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst("UserId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException(MessageConstants.DATA_RETRIEVAL_FAILED);

            var user = await _userGenericRepository.FindFirstAsync(
                u => u.Id == userId,
                u => u,
                true
            );

            if (user == null)
                throw new UnauthorizedAccessException(MessageConstants.DATA_RETRIEVAL_FAILED);

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userGenericRepository.UpdateAsync(user);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            bool user = await _userRepository.ExistsByEmailAsync(email);
            if (!user)
                throw new KeyNotFoundException(MessageConstants.DATA_RETRIEVAL_FAILED);

            string resetUrl = $"http://localhost:4200/reset-password?email={email}";

            await _emailService.SendAsync(
                email,
                "Reset password",
                $@"
            <h3>Welcome to FleetSaaS</h3>
            <p>Please click the button below to reset your password.</p>

            <a href='{resetUrl}'
               style='
                 display:inline-block;
                 padding:12px 20px;
                 background-color:#ef5350;
                 color:#ffffff;
                 text-decoration:none;
                 font-size:16px;
                 font-weight:600;
                 border-radius:6px;
                 font-family:Arial, sans-serif;
               '>
               Reset Password
            </a>

            <p style='margin-top:16px;font-size:13px;color:#666'>
              If you did not request this, please ignore this email.
            </p>
        "
            );
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepository.GetActiveUserWithRolesByEmailAsync(request.Email);

            if (user == null || string.IsNullOrEmpty(user.Password))
                throw new Exception(MessageConstants.DATA_RETRIEVAL_FAILED);

            var result = _passwordHasher.VerifyHashedPassword(
                            user,
                            user.Password,
                            request.Password
                        );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException(MessageConstants.INVALID_CREDENTIALS);

            user.Password = _passwordHasher.HashPassword(user, request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userGenericRepository.UpdateAsync(user);
        }


    }
}
