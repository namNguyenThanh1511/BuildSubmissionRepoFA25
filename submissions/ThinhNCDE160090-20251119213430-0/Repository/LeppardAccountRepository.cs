using BOs;
using Microsoft.EntityFrameworkCore;
using STIService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LeppardAccountRepository : GenericRepository<LeopardAccount>, ILeopardAccountRepository
    {
        public LeppardAccountRepository(Su25leopardDbContext context) : base(context)
        {
        }
        public async Task<LeopardAccount?> Login(string email, string password)
        {
            return await _context.LeopardAccounts
                                 .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
