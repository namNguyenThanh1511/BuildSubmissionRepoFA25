using BLL;
using BLL.ModelView;
using DAL.UserRole;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_SU25_SE173557
{
    [Route("api/")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountBL _systemAccountService;

        public SystemAccountController(ISystemAccountBL systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }


        [HttpPost("auth")]
        public async Task<ActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {

            var account = await _systemAccountService.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                throw new ArgumentException("Invalid email or password.");
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
