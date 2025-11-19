using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PRN232_SU25_SE183867.repository;
using PRN232_SU25_SE183867.service.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_SU25_SE183867.service
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(AuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }


        public async Task<LoginRes> Login(LoginReq loginReq)
        {
            try
            {
                var user = await _authRepository.FindByConditionAsync(x =>
                    x.Email == loginReq.Email && x.Password == loginReq.Password);

                if (user == null) throw new KeyNotFoundException();

                var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(signingCredentials, SecurityAlgorithms.HmacSha256),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginRes
                {
                    Token = tokenHandler.WriteToken(token),
                    Role = user.RoleId.ToString()
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
