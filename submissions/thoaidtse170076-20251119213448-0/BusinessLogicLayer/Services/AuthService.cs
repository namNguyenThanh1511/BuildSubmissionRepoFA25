using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using DataAccessLayer.UOW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        //Hàm sử lý đăng nhập
        public async Task<LeopardAccount> Login(LoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Missing or invalid input", nameof(model));

            var user = _unitOfWork.GetRepository<LeopardAccount>()
                .Entities
                .FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
        
            return user;
        }


        //Hàm xử lí Jwt và response
        public async Task<AuthResponse> GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            //Chỗ sẽ hiển thị ở Response
            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }

    }
}
