using Bisiness.Iservice;
using DataAccess.Dto;
using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bisiness.Service
{
    public class LeopardAccountService : ILeopardAccountService
    {
        public readonly LeopardAccountRepository _leopardAccountRepository;
        private readonly IConfiguration _configuration;

        public LeopardAccountService(LeopardAccountRepository leopardAccountRepository, IConfiguration configuration)
        {
            _leopardAccountRepository = leopardAccountRepository ?? throw new ArgumentNullException(nameof(leopardAccountRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<LoginRespont> login(LoginModelRequest loginModelRequest)
        {
            var temp = await _leopardAccountRepository.GetAsync(loginModelRequest.email, loginModelRequest.password);

            if (temp == null)
            {
                throw new ArgumentException("Invalid email or password");
            }

            string token = GenerateJsonWebToken(temp);

            LoginRespont loginRespont = new LoginRespont
            {
                token = token,
                role = temp.RoleId.ToString()
            };

            return loginRespont;
        }

        private string GenerateJsonWebToken(LeopardAccount systemAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                new Claim[] {
                    new(ClaimTypes.Email, systemAccount.Email),
                    new(ClaimTypes.Role, systemAccount.RoleId.ToString())
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
