using System.Linq;
using System.Threading.Tasks;
using PRN231_SU25_SE181580.DAL.Entities;
using PRN231_SU25_SE181580.BLL.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE181580.BLL.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data;
using PRN231_SU25_SE181580.DAL.Enum;

namespace PRN231_SU25_SE181580.BLL.Implementations {
    public class AuthenticateService: IAuthenticateService {
        private readonly SU25LeopardDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticateService(SU25LeopardDBContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> Login(string email, string password) {
            var account = await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);

            if (account == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = GenerateJwtToken(account);

            var role = string.Empty;

            return new LoginResponseDTO {
                Role = account.RoleId.ToString(),
                Token = token
            };
        }

        private string GenerateJwtToken(LeopardAccount account) {

            var roleName = ((Role) account.RoleId).ToString();
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
