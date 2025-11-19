using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service.Dto;

namespace Service.Impl;

public class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly AuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthService(ILogger<AuthService> logger, AuthRepository authRepository, IConfiguration configuration)
    {
        _logger = logger;
        _authRepository = authRepository;
        _configuration = configuration;
    }


    public async Task<LoginRes> Login(LoginReq loginReq)
    {
        try
        {
            _logger.LogInformation("Login Request");
            var user = await _authRepository.FindByConditionAsync(x =>
                x.Email == loginReq.Email && x.Password == loginReq.Password);

            if (user == null) throw new KeyNotFoundException();

            var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(signingCredentials, SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginRes
            {
                Token = tokenHandler.WriteToken(token),
                Role = user.RoleId.ToString()
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error at login cause by {}", e.Message);
            throw;
        }
    }
}