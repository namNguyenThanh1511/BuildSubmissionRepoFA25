using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184386.DAL.ModelExtensions;
using PRN231_SU25_SE184386.DAL.Models;
using PRN231_SU25_SE184386.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE184386.BLL.Services
{
    public class LeopardAccountService
    {
        private readonly GenericRepository<LeopardAccount> _genericRepository;
        private readonly IConfiguration _configuration;

        public LeopardAccountService(GenericRepository<LeopardAccount> genericRepository, IConfiguration configuration)
        {
            _genericRepository = genericRepository;
            _configuration = configuration;
        }

        public async Task<ApiResponse<TokenResponse>> LoginAsync(string email, string password)
        {
            var response = new ApiResponse<TokenResponse>();
            var user = (await _genericRepository.FindWithIncludeAsync(
                predicate: query => query.Email == email && query.Password == password)).FirstOrDefault();

            if (user == null)
            {
                response.Success = false;   
                response.DetailError = new DetailError
                {
                    ErrorCode = "HB40101",
                    Message = "Account not found or invalid credentials"
                };
                return response;
            }



            var roles = new Dictionary<int, string>
                        {
                            { 1, "admin" },
                            { 2, "manager" },
                            { 3, "staff" },
                            { 4, "member" },
                            { 5, "administrator" },
                            { 6, "moderator" },
                            { 7, "developer" }
                        };

            var token = GenerateAccessToken(roles[user?.RoleId ?? 7]);
            response.Success = true;
            response.Data = new TokenResponse(Token: token, Role: roles[user?.RoleId ?? 7]);
            return response;
        }

        private string GenerateAccessToken(string role)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var claims = new Claim[]
            {
               new(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public record TokenResponse(string Token, string Role);
}
