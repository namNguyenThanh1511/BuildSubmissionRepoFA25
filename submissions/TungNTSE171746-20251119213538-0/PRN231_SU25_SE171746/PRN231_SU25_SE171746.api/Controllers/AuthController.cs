using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE171746.api.Request;
using PRN231_SU25_SE171746.api.Response;
using Repositories.Models;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE171746.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LeopardAccountService _service;
        private readonly IConfiguration _configuration;

        public AuthController(LeopardAccountService userService, IConfiguration configuration)
        {
            _service = userService;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "HB40001",
                    Message = "Email and Password are required"
                });
            }

            var user = await _service.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Email or Password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new AuthResponse
            {
                Token = token,
                Role = user.RoleId.ToString()
            });
        }

        private string GenerateJwtToken(LeopardAccount user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            new Claim("UserId", user.AccountId.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        

    }
}
