using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184736.api.ViewModels;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE184736.api.Controllers
{
    public class AuthenController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AccService _service;

        public AuthenController(IConfiguration configuration, AccService service)
        {
            _configuration = configuration;
            _service = service;
        }

        [HttpPost("api/auth")]
        public async Task<IActionResult> Authen([FromBody] LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodeModel.InvalidInput());
            }

            var user = await _service.Authenticate(request.Email, request.Password);

            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    new Claim[]
                    {
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.RoleId.ToString()!),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    Token = tokenString,
                    Role = user.RoleId.ToString()
                });
            }


            return Unauthorized("Invalid login credentials");

        }
    }
}
