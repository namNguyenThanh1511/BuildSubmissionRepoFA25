using Microsoft.EntityFrameworkCore;
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

        public async Task<LeopardAccount> GetLeopardAccountAsync(string Email, string Password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(
                x => x.Email == Email && x.Password == Password);
        }

    }
}
