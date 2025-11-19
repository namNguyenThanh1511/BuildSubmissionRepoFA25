using Business.Interfaces;
using Business.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly JwtService _jwtService;

        public AuthService(IAccountRepository accountRepo, JwtService jwtService)
        {
            _accountRepo = accountRepo;
            _jwtService = jwtService;
        }

        public LoginResponseDTO Login(LoginRequestDTO request)
        {
            var account = _accountRepo.GetActiveAccountByEmailAndPassword(request.Email, request.Password);

            if (account == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var roleName = RoleMappings.GetRoleName(account.RoleId);
            if (roleName == null)
                throw new UnauthorizedAccessException("Role not allowed");

            var token = _jwtService.GenerateToken(account.Email, roleName);
            return new LoginResponseDTO
            {
                Token = token,
                Role = account.RoleId,
            };
        }
    }
}
