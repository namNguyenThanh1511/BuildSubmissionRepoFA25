using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interface;
using BusinessLogic.ModalViews;
using DataAccess;
using DataAccess.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly ISystemAccountRepository _repository;
        private readonly IConfiguration _config;

        public AuthService(ISystemAccountRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _repository.GetByEmailAndPasswordAsync(request.Email, request.Password);
            if (user == null)
                return null;

           

            // Create token
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var role = user.RoleId.ToString();
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = role
            };
        }
        private string GetRoleName(int? role)
        {
            return role switch
            {
                1 => "administrator",
                2 => "moderator",
                3 => "developer",
                4 => "member",
                _ => "unknown"
            };
        }
    }
}

