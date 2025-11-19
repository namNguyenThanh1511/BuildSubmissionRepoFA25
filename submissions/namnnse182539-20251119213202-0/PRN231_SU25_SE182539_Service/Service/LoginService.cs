using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE182539_Repository.Models;
using Repository.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly SU25LeopardDBContext _context;
        private readonly IConfiguration _config;

        public LoginService(SU25LeopardDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = _context.LeopardAccounts.FirstOrDefault(u =>
                u.Email == request.Email &&
                u.Password == request.Password);

            if (user == null || user.RoleId > 3  || user.RoleId < 8)
            {
                return null;
            }

            var roleName = GetRoleName(user.RoleId);
            var token = GenerateJwtToken(user.Email, roleName);

            return new LoginResponse
            {
                Token = token,
                Role = roleName
            };
        }

        private string GetRoleName(int role)
        {
            return role switch
            {
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                4 => "member",
                _ => "unknown"
            };
        }

        private string GenerateJwtToken(string email, string role)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),


           new Claim(ClaimTypes.Role, role)


        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
