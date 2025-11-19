using BLL.DTOs;
using DAL.Enums;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public class JwtTokenGenerator
    {
        public async Task<AuthenticationModel> CreateToken(LeopardAccount user, JwtSettings jwtSettings)
        {
            jwtSettings.IsValid();

            var now = DateTime.UtcNow;
            var roleName = Enum.GetName(typeof(UserRole), user.RoleId) ?? "Member";

            var claims = new List<Claim>
            {
                new Claim("id", user.AccountId.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("token_type", "access")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthenticationModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = user.RoleId
            };
        }
    }
}
