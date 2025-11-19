using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184930.bll.Interfaces;
using PRN231_SU25_SE184930.dal.DTOs;
using PRN231_SU25_SE184930.dal.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.bll.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILeopardAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(ILeopardAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var account = await _accountRepository.GetByEmailAsync(loginRequest.Email);

            if (account == null || account.Password != loginRequest.Password)
            {
                return null;
            }

            var roleNumber = account.RoleId;
            if (roleNumber == null)
            {
                return null;
            }

            var token = GenerateJwtToken(account.Email, roleNumber);

            return new LoginResponseDto
            {
                Token = token,
                Role = roleNumber
            };
        }

        public string GenerateJwtToken(string email, int role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here-must-be-at-least-32-characters");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"] ?? "leopard-api",
                Audience = _configuration["Jwt:Audience"] ?? "leopard-api"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GetRoleNumber(int? role)
        {
            return role switch
            {
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                4 => "member",
                _ => null
            };
        }
    }
}
