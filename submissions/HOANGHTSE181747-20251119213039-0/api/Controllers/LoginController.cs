using api.dto;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _service;
        public LoginController(IAccountService service)
        {
            _service = service;
        }
        [HttpPost("/api/auth")]
        public async Task<ActionResult<LoginResponseDTO>> LoginJWT(LoginRequestDTO loginRequestDTO)
        {
            var account = await _service.Login(loginRequestDTO.Email, loginRequestDTO.Password);
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);
            var role = account.RoleId.ToString();
            return Ok(new LoginResponseDTO
            {
                Role = role,
                Token = token,
            });
        }
    }
}
