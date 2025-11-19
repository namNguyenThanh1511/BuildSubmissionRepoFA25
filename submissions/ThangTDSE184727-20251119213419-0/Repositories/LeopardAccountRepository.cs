using Repositories.Base;
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
        public async Task<LeopardAccount> GetLeopardAccount(string email, string password)
        {
            return _context.LeopardAccounts.FirstOrDefault(l => l.Email == email && l.Password == password);
        }
    }
}
