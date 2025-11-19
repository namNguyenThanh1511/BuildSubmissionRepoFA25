using BLL.DTOs;
using BLL.Responses;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService
    {
        private readonly JwtService _jwtService;
        private readonly AccountRepository _accountRepository;
        public AccountService(AccountRepository accountRepository, JwtService jwtService)
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO loginDto)
        {
            var account = await _accountRepository.LoginAsync(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if(account.RoleId != 5 && account.RoleId != 7 && account.RoleId != 6 && account.RoleId != 4)
            {
                string token1 = "null";
                return new LoginResponse { token = token1, role = account.RoleId.ToString() };
            }

            var token = _jwtService.GenerateJwtToken(account);

            return new LoginResponse { token = token, role = account.RoleId.ToString() };
        }
    }
}
