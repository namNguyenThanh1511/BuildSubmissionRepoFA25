using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Dto;
using Repository.Models;

namespace PRN231_SU25_SE183870.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;



        public AuthController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var account = await _accountService.Authenticate(request.Email, request.Password);

            if (account == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Invalid credentials"
                });
            }

            if (account.RoleId ==null)
            {
                return StatusCode(403, new
                {
                    errorCode = "HB40301",
                    message = "Permission denied"
                });
            }
            var roleName = ConvertRoleToString(account.RoleId);
            if (roleName == "guest")
            {
                return StatusCode(403, new
                {
                    errorCode = "HB40301",
                    message = "Permission denied"
                });
            }

            var token = GenerateJwtToken(account, roleName);

            return Ok(new
            {
                token = token,
                role = account.RoleId
            });
        }
        private string ConvertRoleToString(int? role)
        {
            return role switch
            {
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                4 => "member",
                _ => "guest",
            };
        }
        private string GenerateJwtToken(LeopardAccount account, string roleName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("AccountId", account.AccountId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
