using BLL.DTOs;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
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
        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            var account = await _repo.GetAccountAsync(request.Email, request.Password);
            if (account == null || account.RoleId == null)
                return null;

            var roleName =account.RoleId;
            

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, account.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }
    }
}
