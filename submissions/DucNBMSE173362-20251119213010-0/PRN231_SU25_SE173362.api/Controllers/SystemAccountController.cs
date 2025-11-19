using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE173362.BLL;
using PRN231_SU25_SE173362.DAL.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE173362.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SystemAccountService _service;

        public SystemAccountController(IConfiguration config, SystemAccountService service)
        {
            _config = config;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
           
            var user = await _service.LogIn(request.Email, request.Password);

            if (user == null)
            {
                return ErrorHelper.BadRequestResult("leopardAccount not existed");
            }

            var token = GenerateJSONWebToken(user);

           

            return Ok(new
            {
                token = token,
                role = user.RoleId
            });
        }

        private string GenerateJSONWebToken(LeopardAccount account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
            new(ClaimTypes.Name, account.Email),
            new(ClaimTypes.Role, account.RoleId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public sealed record LoginRequest(string Email, string Password);
    }
}
