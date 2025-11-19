using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SU25Leopard.BusinessObject.DTO;
using SU25Leopard.BusinessObject.Models;
using SU25Leopard.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SU25Leopard.BusinessLogicLayer.Services
{
    public class LeopardAccountService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public LeopardAccountService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<LoginResponseDTO> Login(string email, string password)
        {
            var user = await _unitOfWork.LeopardAccountRepository.GetSingleByConditionAsync(x => x.Email == email && x.Password == password);
            if (user == null)
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return new LoginResponseDTO
            {
                Token = token,
                Role = user.RoleId.ToString()
            };
        }
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
