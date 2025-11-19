using Microsoft.IdentityModel.Tokens;
using PRN232_SU23_SE170578.api.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_SU23_SE170578.api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<(string Token, string Role)?> Authenticate(string email, string password)
        {
            var user = await _repo.GetUserByEmailAndPassword(email, password);
            if (user == null) return null;

            if (user.RoleId != 1 && user.RoleId != 2 &&
                user.RoleId != 3 && user.RoleId != 4)
                return null;

            // Generate JWT token
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, user.RoleId.ToString());
        }
    }
}
