using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public interface ILeopardAccountRepository
    {
        Task<LeopardAccount> Login(string email, string password);
    }

    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private static LeopardAccountRepository? instance = null;

        private LeopardAccountRepository() { }

        public static LeopardAccountRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardAccountRepository();
                }
                return instance;
            }
        }
        public async Task<LeopardAccount> Login(string email, string password)
        {
            using (var context = new SU25LeopardDBContext())
            {
                var account = await context.LeopardAccounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
                return account;
            }
        }
    }
}
