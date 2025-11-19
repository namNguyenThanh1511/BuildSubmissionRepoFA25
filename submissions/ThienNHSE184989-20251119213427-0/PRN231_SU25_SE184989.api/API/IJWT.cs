using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API
{
    public interface IJWT
    {
        string GenerateToken(LeopardAccount account, int role);
    }

    public class JWT : IJWT
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretkey;

        public JWT(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretkey = _configuration["JWT:SecretKey"];
        }

        public string GenerateToken(LeopardAccount account, int role)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretkey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role+""),
                new Claim(ClaimTypes.Email, account.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }

    }

}
