using BusinessObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class LeopardAccountDAO
    {
        private static LeopardAccountDAO instance = null;
        private readonly SU25LeopardDBContext context;

        private LeopardAccountDAO()
        {
            context = new SU25LeopardDBContext();
        }

        public static LeopardAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardAccountDAO();
                }
                return instance;
            }
        }

        public List<LeopardAccount> GetLeopardAccount()
        {
            var accountList = new List<LeopardAccount>();

            var adminAccount = GetAdminAccount();
            accountList.Add(adminAccount);

            var dbAccounts = context.LeopardAccounts
                                    .Where(account => account.Email != adminAccount.Email)
                                    .ToList();
            accountList.AddRange(dbAccounts);

            return accountList;
        }

        public LeopardAccount GetLeopardAccountById(int id)
        {
            return context.LeopardAccounts.FirstOrDefault(account => account.AccountId == id);
        }

        public LeopardAccount Login(string email, string password)
        {
            List<LeopardAccount> accounts = GetLeopardAccount();
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
                UserName = "admin",
                Email = configuration["AdminAccount:Email"],
                Password = configuration["AdminAccount:Password"],
                RoleId = 1
            };

            return adminAccount;
        }
    }
}
