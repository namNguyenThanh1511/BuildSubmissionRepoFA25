using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Basic;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardAccountRepository : GenericRepository<LeopardAccount>
    {
        public LeopardAccountRepository() { }

        public async Task<List<LeopardAccount>> GetLeopardAccounts()
        {
            var accountList = new List<LeopardAccount>();

            var adminAccount = GetAdminAccount();
            accountList.Add(adminAccount);

            var dbAccounts = await _context.LeopardAccounts
                                    .Where(account => account.Email != adminAccount.Email)
                                    .ToListAsync();
            accountList.AddRange(dbAccounts);

            return accountList;
        }

        public async Task<LeopardAccount> GetLeopardAccountById(int id)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(account => account.AccountId == id);
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            List<LeopardAccount> accounts = await GetLeopardAccounts();
            LeopardAccount account = accounts
                                    .Where(account => account.Email == email && account.Password == password)
                                    .FirstOrDefault();
            return account;
        }

        private LeopardAccount GetAdminAccount()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

            var adminAccount = new LeopardAccount
            {
                AccountId = 1,
                FullName = "admin456",
                Email = configuration["AdminAccount:Email"],
                Password = configuration["AdminAccount:Password"],
                RoleId = 1
            };

            return adminAccount;
        }
    }
}
