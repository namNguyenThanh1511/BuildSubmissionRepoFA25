using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232_SU25_SE181544.BLL.DTOs;
using PRN232_SU25_SE181544.DAL.Models;
using PRN232_SU25_SE181544.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE181544.BLL.Services
{
    public class LeopardAccountService
    {
        private readonly IConfiguration _configuration;
        public LeopardAccountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private LeopardAccountRepository repository=new();

        public async Task<LoginResponseDto> Login(string email, string password)
        {
            var account = await repository.Login(email, password);
            if (account == null)
            {
                return null;
            }
            var token = GenerateJwtToken(account);
            return new LoginResponseDto
            {
                Token = token,
                Role = account.RoleId.ToString(),
            };
        }
        //private string GetRoleName(int? role)
        //{
        //    return role switch
        //    {
        //        1 => "administrator",
        //        2 => "moderator",
        //        3 => "developer",
        //        4 => "member",
        //        _ => "unknown"
        //    };
        //}
        private string GenerateJwtToken(LeopardAccount user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT configuration is incomplete.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
