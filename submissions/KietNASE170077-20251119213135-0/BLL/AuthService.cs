using DAL.Entities;
using DAL.Model;
using DAL.UOW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL
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

        public async Task<LeopardAccount> Login(LoginModel model)
        {
            var user = _unitOfWork.GetRepository<LeopardAccount>()
                .Entities
                .FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
            if (user == null)
            {
                return null;
            }

            // Chuyển role từ int sang string (nếu cần phân quyền bằng chuỗi)
            string roleName = ConvertRoleToString(user.RoleId);
            if (roleName == null)
                return null; // Không cấp token nếu role không hợp lệ

            return user;
        }
        private string? ConvertRoleToString(int? role)
        {
            return role switch
            {
                5 => "5",
                6 => "6",
                7 => "7",
                4 => "4",
                _ => null
            };
        }

        public async Task<AuthResponse> GenerateJwtToken(string username, int? roleUser)
        {
            var role = ConvertRoleToString(roleUser);
            // Tạo danh sách các claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),  // Tên người dùng
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // ID của token
                new Claim(ClaimTypes.Role, role)  // Vai trò của người dùng
            };

            // Lấy Secret từ cấu hình
            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);

            // Tạo SigningCredentials từ khóa bí mật (Secret) và thuật toán HMACSHA256
            var creds = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);

            // Tạo JWT token với các tham số: Issuer, Audience, Claims, TimeSpan và SigningCredentials
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Trả về token dưới dạng chuỗi JWT
            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = role,
            };
        }
    }
}
