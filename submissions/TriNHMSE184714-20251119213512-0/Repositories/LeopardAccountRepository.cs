using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardAccountRepository : GenericRepository<LeopardAccount>
    {
        public LeopardAccountRepository()
        {

        }

        public async Task<LeopardAccount?> GetAccountAsync(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(x => x.Email == email
                                                                      && x.Password == password);
        }
    }
}
