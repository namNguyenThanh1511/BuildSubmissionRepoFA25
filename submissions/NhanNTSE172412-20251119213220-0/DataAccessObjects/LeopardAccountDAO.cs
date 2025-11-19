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

        public List<LeopardAccount> GetLeopardAccounts()
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

        public LeopardAccount Login(string email, string password)
        {
            List<LeopardAccount> accounts = GetLeopardAccounts();
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
                AccountId = 5,
                UserName = "administrator",
                Email = configuration["AdminAccount:Email"],
                Password = configuration["AdminAccount:Password"],
                RoleId = 5
            };

            return adminAccount;
        }
    }
}
