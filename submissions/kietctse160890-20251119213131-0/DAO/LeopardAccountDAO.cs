using BO;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class LeopardAccountDAO
    {
        private static LeopardAccountDAO instance = null;
        private readonly Su25leopardDbContext context;

        private LeopardAccountDAO()
        {
            context = new Su25leopardDbContext();
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
        public async Task<LeopardAccount> Login(string email, string password)
        {
            var account = await context.LeopardAccounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
            return account;
        }
    }
}
