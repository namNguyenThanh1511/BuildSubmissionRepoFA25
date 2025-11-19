using Repository.Interface;
using Repository.Models;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepository; 

		public AccountService (IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public Task<LeopardAccount> Login(string email, string password)
		{
			return _accountRepository.Login(email, password);
		}
	}
}
