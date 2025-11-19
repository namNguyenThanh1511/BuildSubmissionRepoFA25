using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184714.api.Models;
using Repositories.Models;
using Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE184714.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LeopardAccountService _service;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _service = new LeopardAccountService();
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodeModel.Invalid());
            }

            var acc = await _service.Authenticate(request.Email, request.Password);
            if (acc == null)
                return Unauthorized(ErrorCodeModel.Unauthor());

            var token = GenerateJSONWebToken(acc);

            return Ok(new
            {
                token,
                role = acc.RoleId.ToString()
            });
        }
        private string GenerateJSONWebToken(LeopardAccount acc)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
                    new(ClaimTypes.Name, acc.FullName),
                    new(ClaimTypes.Role, acc.RoleId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }

    }
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}