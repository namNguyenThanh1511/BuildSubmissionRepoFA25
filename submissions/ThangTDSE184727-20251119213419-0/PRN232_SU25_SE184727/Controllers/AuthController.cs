using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN232_SU25_SE184727.Models;
using Repositories.Models;
using Services;
using Services.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_SU25_SE184727.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LeopardAccountService _leopardAccountService;
        private readonly IConfiguration _configuration;

        public AuthController(LeopardAccountService leopardAccountService, IConfiguration configuration)
        {
            _leopardAccountService = leopardAccountService;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModelRequest loginModelRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorModel.Invalid());
            }
            var account = await _leopardAccountService.Authenticate(loginModelRequest.email, loginModelRequest.password);
            if (account == null)
            {
                return NotFound(ErrorModel.NotFound());
            }
            var token = GenerateJSONWebToken(account);
            return Ok(new
            {
                token,
                role = account.RoleId.ToString(),
            });
        }
        private string GenerateJSONWebToken(LeopardAccount leopardAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]
                , _configuration["Jwt:Audience"]
                , new Claim[]
                {
                    new (ClaimTypes.Name, leopardAccount.FullName),
                    new (ClaimTypes.Role, leopardAccount.RoleId.ToString())
                },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
