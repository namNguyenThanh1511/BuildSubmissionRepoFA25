
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE172431.BLL.DTO.Request;
using PRN231_SU25_SE172431.BLL.DTO.Response;
using PRN231_SU25_SE172431.DAL.Data;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.BLL.Service
{
    public interface ILeoPardAccountService
    {
        public Task<LoginResponse> Authentication(LoginRequest request);
    }
    public class LeoPardAccountService : ILeoPardAccountService
    {
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public LeoPardAccountService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Authentication(LoginRequest request)
        {

            var account = await _unitOfWork.LeopardAccountRepository.Entities.FirstOrDefaultAsync(x => x.Email.ToLower() == request.email.ToLower() && x.Password.ToLower() == request.password.ToLower());

            if (account == null)
                throw new Exception("Invalid email or password");

            var token = GenerateJsonWebToken(account);

            var result = new LoginResponse()
            {
                Token = token,
                Role = account.RoleId,

            };
            return result;

        }
        private string GenerateJsonWebToken(LeopardAccount systemAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                new Claim[] {
                    new(ClaimTypes.Email, systemAccount.Email)
                , new(ClaimTypes.Role, systemAccount.RoleId.ToString())
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
