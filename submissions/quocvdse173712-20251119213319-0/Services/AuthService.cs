using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{

public class AuthService
{
    private readonly ILeopardAccountRepo _repo;
    private readonly IConfiguration _config;

    public AuthService(ILeopardAccountRepo repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _repo.LoginAsync(request.Email, request.Password);
        if (user == null) return null;

        var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Token = token,
                Role = (int)user.RoleId,
            };
    }

    private string GenerateJwtToken(LeopardAccount user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, GetRoleName((int)user.RoleId))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetRoleName(int role)
    {
        return role switch
        {
            5 => "administrator",
            6 => "moderator",
            7 => "developer",
            4 => "member",
            _ => "unknown"
        };
    }
}

}
