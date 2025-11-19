using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly IConfiguration _config;
        private readonly ILeopardAccountRepository _repo;

        public LeopardAccountService(ILeopardAccountRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<AuthResponse?> AuthenticateAsync(string email, string password)
        {
            LeopardAccount? acct = await _repo.LoginAsync(email, password);
            if (acct == null)
                return null;

            string? roleName = acct.RoleId switch
            {
                5 => "administrator", 
                6 => "moderator",      
                7 => "developer",      
                4 => "member",        
                _ => null               
            };
            if (roleName == null)
                return null;

            var jwtCfg = _config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtCfg["Key"]!);
            var issuer = jwtCfg["Issuer"]!;
            var audience = jwtCfg["Audience"]!;
            int days = int.Parse(jwtCfg["ExpiryInDays"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, acct.AccountId.ToString()),
                new Claim(ClaimTypes.Email,          acct.Email),
                new Claim(ClaimTypes.Role,           roleName)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(days),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDesc);
            string jwt = handler.WriteToken(token);

            return new AuthResponse
            {
                Token = jwt,
                Role = acct.RoleId
            };
        }
    }
}
