using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE173171.BLL.DTOs;
using PRN231_SU25_SE173171.BLL.Interfaces;
using PRN231_SU25_SE173171.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _repo;

        public LeopardAccountService(ILeopardAccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _repo.GetUser(request.Email, request.Password);
            if (user == null)
            {
                throw new Exception("Email or Password is wrong!");
            } else
            {

                IConfiguration configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", true, true).Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                };

                var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var signCredential = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                var preparedToken = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signCredential);

                var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);

                return new LoginResponse { Token = generatedToken, Role = user.RoleId.ToString() };
            }
        }
    }
}
