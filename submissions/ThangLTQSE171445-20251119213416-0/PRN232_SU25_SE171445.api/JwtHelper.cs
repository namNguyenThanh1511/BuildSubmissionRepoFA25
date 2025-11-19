using Microsoft.IdentityModel.Tokens;
using Repositories.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_SU25_SE171445.api
{
    public static class JwtHelper
    {
        public static string GenerateToken(LeopardAccount user, IConfiguration configuration)
        {
            var roleName = user.RoleId switch
            {
                4 => "member",
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                _ => null
            };

            if (roleName == null)
                return null;

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("UserId", user.AccountId.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
