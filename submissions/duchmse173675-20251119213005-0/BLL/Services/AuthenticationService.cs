using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository<LeopardAccount> _repository;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthenticationService(IGenericRepository<LeopardAccount> repository, JwtSettings jwtSettings, JwtTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _jwtSettings = jwtSettings;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<AuthenticationModel> LoginWithEmailPasswordAsync(string email, string password)
        {
            var user = _repository.GetAll().FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = await _tokenGenerator.CreateToken(user, _jwtSettings);

            return new AuthenticationModel
            {
                Token = token.Token,
                Role = token.Role
            };
        }
    }
}
