using BLL.DTOs;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly ILeopardAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AuthService(ILeopardAccountRepository accountRepository, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var isValid = await _accountRepository.ValidateCredentialsAsync(request.Email, request.Password);
        if (!isValid)
            return null;

        var account = await _accountRepository.GetByEmailAsync(request.Email);
        if (account == null || account.RoleId == null)
            return null;

        var roleName = GetRoleName(account.RoleId);
        if (string.IsNullOrEmpty(roleName))
            return null;

        var token = GenerateJwtToken(account, roleName);

        return new LoginResponse
        {
            Token = token,
            Role = roleName
        };
    }

    public string GetRoleName(int roleId)
    {
        return roleId switch
        {
            1 => "admin",
            2 => "manager",
            3 => "staff",
            4 => "member",
            5 => "administrator",
            6 => "moderator",
            7 => "developer",
            _ => string.Empty
        };
    }

    private string GenerateJwtToken(DAL.Models.LeopardAccount account, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "1iu3hh2q4safd802304920390sa09df09jsaf1234i23h4h234213j4khkjsdhf"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}