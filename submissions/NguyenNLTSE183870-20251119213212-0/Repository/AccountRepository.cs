using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository
{
    public class AccountRepository : GenericRepository<LeopardAccount>
    {
        public AccountRepository()
        {
        }

        public async Task<LeopardAccount?> GetAccountAsync(string a, string b)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(x => x.Email == a &&
                                                                         x.Password == b);
        }
    }
}
