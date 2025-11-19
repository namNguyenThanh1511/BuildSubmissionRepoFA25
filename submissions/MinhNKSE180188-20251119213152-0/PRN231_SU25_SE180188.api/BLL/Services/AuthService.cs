using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;

        public AuthService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<(LeopardAccount Account, string ErrorCode, string ErrorMessage)> ValidateLoginAsync(string email, string password)
        {
            var account = await _accountRepository.GetByEmailPasswordAsync(email, password);
            if (account == null)
                return (null, "HB40101", "Invalid credentials or inactive account");

            if (account.RoleId != 4 && account.RoleId != 5 && account.RoleId != 6 && account.RoleId != 7)
                return (null, "HB40301", "Role not allowed");

            return (account, null, null);
        }
    }
}
