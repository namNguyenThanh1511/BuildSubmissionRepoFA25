using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardAccountRepo : GenericRepository<LeopardAccount>
    {
        public LeopardAccountRepo() { }
        public async Task<LeopardAccount> GetUserAccountAsync(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(u => u.Email == email
            && u.Password == password);
        }
    }
}

