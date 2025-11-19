using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE172411.api.Controllers
{
    [Route("api")]
    [ApiController]
    public class LeopardAccountController : ControllerBase
    {
        private readonly ILeopardAccountService _service;
        private readonly IConfiguration configuration;

        public LeopardAccountController(ILeopardAccountService service, IConfiguration config)
        {
            _service = service;
            configuration = config;
        }

        [HttpPost("auth")]
        public IActionResult Login([FromBody] LeopardAccountRequestDTO leopardAccountRequestDTO)
        {
            var account = _service.Login(leopardAccountRequestDTO.Email, leopardAccountRequestDTO.Password);
            if (account == null)
            {
                return Unauthorized(new { errorCode = "HB40101", message = "Token missing / invalid" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("RoleId", account.RoleId.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim("UserName", account.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                RoleId = account.RoleId.ToString(),
            });
        }

        [HttpGet("User")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var RoleId = User.FindFirst("RoleId")?.Value;
            var accountId = User.FindFirst("AccountId")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(RoleId) || string.IsNullOrEmpty(accountId))
                return Unauthorized(new { errorCode = "HB40101", message = "Token missing / invalid" });

            return Ok(new
            {
                Email = email,
                Role = RoleId,
            });
        }
    }
}
