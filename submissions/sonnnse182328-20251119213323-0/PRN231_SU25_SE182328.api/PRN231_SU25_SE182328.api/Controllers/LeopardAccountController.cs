using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE182328.api.Controllers
{
    [ApiController]
    [Route("api")]
    public class LeopardAccountController : BaseController
    {
        private readonly ILeopardAccountService _LeopardAccountService;
        private readonly IConfiguration _configuration;

        public LeopardAccountController(ILeopardAccountService LeopardAccountService, IConfiguration configuration)
        {
            _LeopardAccountService = LeopardAccountService;
            _configuration = configuration;
        }

        public class AuthRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            try
            {
                var account = _LeopardAccountService.GetLeopardAccountByIdAsync(request.Email, request.Password);
                if (account == null)
                    return Error("AUTH001", "Invalid email or password or account is inactive", 401);

                string roleName = account.RoleId switch
                {
                    5 => "administrator",
                    6 => "moderator",
                    7 => "developer",
                    4 => "member",
                    _ => null
                };

                if (roleName == null)
                    return Error("AUTH003", "Invalid account role", 401);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, account.FullName),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, roleName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    role = roleName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][Authenticate] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }
    }
}
