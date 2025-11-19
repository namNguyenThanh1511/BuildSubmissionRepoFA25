using DataAccess.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class LeopardAccountService : ILeopardAccountService
    {

        private readonly ILeopardAccountRepository _accountRepository;

        public LeopardAccountService(ILeopardAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<LeopardAccount?> LoginAsync(string email, string password)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account == null || account.Password != password)
            {
                return null;
            }

            return account;
        }
    }
}
