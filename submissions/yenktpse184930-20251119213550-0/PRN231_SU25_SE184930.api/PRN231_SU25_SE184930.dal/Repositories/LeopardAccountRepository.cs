using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184930.dal.DBContext;
using PRN231_SU25_SE184930.dal.Interfaces;
using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardAccountRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> GetByEmailAsync(string email)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<LeopardAccount> GetByIdAsync(int id)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(x => x.AccountId == id);
        }
    }
}
