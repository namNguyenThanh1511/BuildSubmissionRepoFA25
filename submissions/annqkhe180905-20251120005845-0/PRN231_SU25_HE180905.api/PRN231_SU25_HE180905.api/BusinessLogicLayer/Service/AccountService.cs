using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using DataAccessLayer.ReqAndRes;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IConfiguration _config;

        public AccountService(IAccountRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public AuthResponse Authenticate(string email, string password)
        {
            LeopardAccount acct = _repo.GetByEmailAndPassword(email, password);
            if (acct == null) return null;

            string roleName = acct.RoleId switch
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
            var keyBytes = Encoding.UTF8.GetBytes(jwtCfg["Key"]);
            var issuer = jwtCfg["Issuer"];
            var audience = jwtCfg["Audience"];
            int days = int.Parse(jwtCfg["ExpiryInDays"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, acct.AccountId.ToString()),
                new Claim(ClaimTypes.Email,          acct.Email),
                new Claim(ClaimTypes.Role,           roleName)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
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
                Role = roleName
            };
        }
    }
}