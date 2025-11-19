using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Model;
using Repository.Models;


namespace Service
{
    public class AuthService
    {

        private readonly UnitOfWork _unitOfWork;

        public AuthService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginReponse?> Login(LoginDTO loginDTO)
        {
            var account = _unitOfWork.GetRepository<LeopardAccount>()
                .GetByPropertyAsync(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password, tracked: false)
                .Result;
            if (account == null)
            {
                return null;
            }

            //Generate JWT Token
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("role", account.RoleId.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"])),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);

           return  new LoginReponse
           {
               Token = token,
               Role = account.RoleId,
           };
        }
    }
}
