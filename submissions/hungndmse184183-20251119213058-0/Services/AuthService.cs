using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.DTOs;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _accountRepository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository accountRepository, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _config = config;
        }

        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            var user = await _accountRepository.GetActiveUserByEmailAndPasswordAsync(email, password);
            if (user == null) return null;

            var roleName = AuthHelper.GetRoleName(user.RoleId);
            if (roleName == null) return null;

            var token = AuthHelper.GenerateToken(email, roleName, _config);
            return new LoginResponse { Token = token, Role = roleName };
        }
    }
}
