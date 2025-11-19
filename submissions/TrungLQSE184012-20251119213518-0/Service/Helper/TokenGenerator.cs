using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repo.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper
{
    public interface ITokenGenerator
    {
        string GenerateToken(LeopardAccount user);
    }

    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string ITokenGenerator.GenerateToken(LeopardAccount user)
        {
            var date = DateTime.UtcNow;
            TimeZoneInfo asianZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            date = TimeZoneInfo.ConvertTimeFromUtc(date, asianZone);
            Console.WriteLine(date);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId != 0? user.RoleId.ToString() : "4"),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:Issuer"],
                audience: _configuration["JwtToken:Audience"],
                claims: claims,
                expires: date.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
