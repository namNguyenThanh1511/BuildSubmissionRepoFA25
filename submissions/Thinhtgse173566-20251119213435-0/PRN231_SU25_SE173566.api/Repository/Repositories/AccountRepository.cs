using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly SU25LeopardDBContext _context;

		public AccountRepository (SU25LeopardDBContext context)
		{
			_context = context;
		}

		public  Task<LeopardAccount> Login(string email, string password)
		{
			return _context.LeopardAccounts
					.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);

		}


	}
}
