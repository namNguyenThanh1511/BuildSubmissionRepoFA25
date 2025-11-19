using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess;
using System;
using DataAccess.Context;

namespace PRN231_SU25_SE183936.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Su25leopardDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(Su25leopardDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.LeopardAccounts
                .FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Invalid email or password"
                });
            }


            var roleName = GetRoleName(user.RoleId);
            if (roleName == null)
            {
                return StatusCode(403, new
                {
                    errorCode = "HB40301",
                    message = "Permission denied"
                });
            }

            var token = GenerateJwtToken(user.Email, roleName);

            return Ok(new
            {
                token = token,
                role = roleName
            });
        }

        private string? GetRoleName(int roleId)
        {
            return roleId switch
            {
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                4 => "member",
                _ => null
            };
        }

        private string GenerateJwtToken(string email, string roleName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
