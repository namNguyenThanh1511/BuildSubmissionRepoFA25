using BusinessLogic.Service;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Presentation.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService systemAccountService)
        {
            _userService = systemAccountService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {
            try
            {
                var account = await _userService.Login(loginDTO.Email, loginDTO.Password);
                if (account == null)
                {
                    return ErrorHelper.BadRequest("Invalid email or password");
                }

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("Role", account.RoleId.ToString()),
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
                var role = account.RoleId.ToString();
                var accountId = account.AccountId.ToString();

                return Ok(new AccountResponseDTO
                {
                    Role = role,
                    Token = generatedToken,
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
