using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE172399.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardAccountsController : ControllerBase
    {
        private readonly ILeopardAccountService service;
        private readonly IConfiguration configuration;

        public LeopardAccountsController(ILeopardAccountService service, IConfiguration config)
        {
            this.service = service;
            configuration = config;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var account = await service.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized(new { errorCode = "HB40101", message = "Invalid email or password" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("Role", account.RoleId.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim("AccountName", account.FullName)
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
                role = account.RoleId.ToString(),
                //accountId = account.AccountId
            });
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst("Role")?.Value;
            var accountId = User.FindFirst("AccountId")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(accountId))
                return Unauthorized(new { errorCode = "HB40101", message = "User not authenticated" });

            return Ok(new
            {
                Email = email,
                Role = role,
                AccountId = accountId
            });
        }
    }
}
