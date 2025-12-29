using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Enum;
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

            return new LoginResponse
            {
                AccessToken = token,
                ExpiresAt = DateTime.UtcNow.AddHours(7)
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
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
