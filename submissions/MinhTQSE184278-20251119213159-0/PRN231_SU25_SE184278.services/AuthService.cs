using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184278.repositories;
using PRN231_SU25_SE184278.services.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            var account = await _repo.GetAccountAsync(request.Email, request.Password);
            if (account == null)
                return null;

            //var roleName = GetRoleName(account.Role.Value);
            //if (string.IsNullOrEmpty(roleName))
            //    return null;

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, account.RoleId.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = account.RoleId
            };
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "administrator",
                2 => "moderator",
                3 => "developer",
                4 => "member",
                _ => string.Empty
            };
        }
    }
}
