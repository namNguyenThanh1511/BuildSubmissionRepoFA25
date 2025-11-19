using DataAccess.Base;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardAccountRepository : GenericRepository<LeopardAccount>
    {
        public async Task<LeopardAccount> GetAsync(string email,string password)
        {
            return await _context.LeopardAccounts
                                        .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
