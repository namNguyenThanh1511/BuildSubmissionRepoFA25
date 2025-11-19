using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LoginService
    {
        private readonly SU25LeopardDBContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public LoginService()
        {
        }

        public LoginService(SU25LeopardDBContext dbContext, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginDTO> Login(string email, string password)
        {
            var user = await _dbContext.LeopardAccounts.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //var roles = _userManager.GetRolesAsync(user).Result;
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var secretKey = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT Secret Key is missing in appsettings.json.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new LoginDTO { token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor), role = user.RoleId };
        }

    }
}
