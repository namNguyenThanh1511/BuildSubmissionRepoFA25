using BOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;
        private readonly Su25leopardDbContext context;

        private AccountDAO()
        {
            context = new Su25leopardDbContext();
        }

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            var account = await context.LeopardAccounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
            return account;
        }

    }
}
