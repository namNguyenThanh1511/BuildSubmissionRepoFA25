using DAO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILeopardAccountService _systemAccountService;

        public AuthController(ILeopardAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var account = await _systemAccountService.Login(request.Email, request.Password);
                if (account == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("Role", account.RoleId.ToString()),
                    new Claim("AccountId", account.AccountId.ToString()),
                };

                var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var signCredential = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                var preparedToken = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(16),
                    signingCredentials: signCredential);

                var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);
                var role = account.RoleId;
                var accountId = account.AccountId;

                return Ok(new LoginResponse
                {
                    Token = generatedToken,
                    RoleName = role.ToString()
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    errorCode = "HB50001",
                    message = "Permission denied"
                });
            }
        }
    }
}
