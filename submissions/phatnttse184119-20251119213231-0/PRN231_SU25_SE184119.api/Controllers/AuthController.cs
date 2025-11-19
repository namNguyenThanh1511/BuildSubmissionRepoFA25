using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184119.Repositories.Models;
using PRN231_SU25_SE184119.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE184119.api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILeopardAccountService _LeopardAccountService;

        public AuthController(IConfiguration configuration, ILeopardAccountService LeopardAccountService)
        {
            _configuration = configuration;
            _LeopardAccountService = LeopardAccountService;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _LeopardAccountService.LoginRequest(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            var token = GenerateJSONWebToken(user);
            return Ok
                (new
                {
                    token,
                    role = user.RoleId

                });


        }

        private string GenerateJSONWebToken(LeopardAccount LeopardUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Map RoleId to enum name if possible
            string roleName = Enum.IsDefined(typeof(RoleEnum), LeopardUserAccount.RoleId)
                ? ((RoleEnum)LeopardUserAccount.RoleId).ToString()
                : "Unknown";

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, LeopardUserAccount.UserName),
        new Claim(ClaimTypes.Email, LeopardUserAccount.Email),
        new Claim(ClaimTypes.Role, roleName)
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
