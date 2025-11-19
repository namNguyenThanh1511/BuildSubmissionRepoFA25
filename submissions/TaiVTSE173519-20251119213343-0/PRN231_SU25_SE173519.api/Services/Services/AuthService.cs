using Repositories.Interfaces;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _jwtSettings;

        public AuthService(IAuthRepository authRepository, TokenService jwtSettings)
        {
            _authRepository = authRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<LoginResponseDTO?> Login(LoginRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return null;

            var user = await _authRepository.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
            if (user == null) return null;


            string role = user.RoleId switch
            {
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                6 => "6",
                7 => "7",
                _ => null
            };

            if (string.IsNullOrEmpty(role)) return null;

            var token = _jwtSettings.GenerateToken(user.AccountId.ToString(), role);

            return new LoginResponseDTO
            {
                Role = role,
                Token = token
            };
        }
    }
}
