
using DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UOW;

namespace Services
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

        public async Task<LeopardAccount?> Login(LoginModel model)
        {
            try
            {
                var user = _unitOfWork.GetRepository<LeopardAccount>()
                .Entities
                .FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);

                if(user == null)
                {
                    return null;
                }
            string? roleName = ConvertRoleToString(user.RoleId);
            if (roleName == null)
                return null; 

            return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string? ConvertRoleToString(int role)
        {
            try { 
                string rolename =  role  switch
                {
                    4 => "member",
                    5 => "administrator",
                    6 => "moderator",
                    7 => "developer",
                    _ => null
                    
                };
                return rolename;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuthResponse> GenerateJwtToken(string username, int roleUser)
        {
            var role = ConvertRoleToString(roleUser);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role), 
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

            return new AuthResponse 
            { 
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = role,
            };
        }

    }
}
