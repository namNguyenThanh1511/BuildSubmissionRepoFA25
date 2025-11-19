using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL;
using BLL.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PRN232_SU25_221179_SE173565.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountBL _accountService;

        public AuthController(IAccountBL systemAccountService)
        {
            _accountService = systemAccountService;
        }


        [HttpPost("auth")]
        public async Task<ActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {

            var account = await _accountService.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                throw new ArgumentException("Invalid email or password.");
            }
            if (account.RoleId == 1 || account.RoleId == 2 || account.RoleId == 3)
            {
                throw new ForbiddenAccessException("You are not allowed to access this function!");
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
                Token = generatedToken
            });

        }

    }
}
