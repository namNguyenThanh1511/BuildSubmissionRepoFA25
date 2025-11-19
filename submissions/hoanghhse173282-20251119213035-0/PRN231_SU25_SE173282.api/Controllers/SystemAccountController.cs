using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE173282.api.Error;
using PRN231_SU25_SE173282.BLL;
using PRN231_SU25_SE173282.DAL.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        string role = "";
        var user = await _service.LogIn(request.Email, request.Password);

        if (user == null)
        {
            return ErrorHelper.BadRequestResult("User not existed");
        }

        var token = GenerateJSONWebToken(user);

        switch (user.RoleId)
        {
            case 1:
                role = "1";
                break;
            case 2:
                role = "2";
                break;
            case 3:
                role = "3";
                break;
            case 4:
                role = "4";
                break;
            case 5:
                role = "5";
                break;
            case 6:
                role = "6";
                break;
            case 7:
                role = "7";
                break;
        }

        return Ok(new
        {
            token = token,
            role = role
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