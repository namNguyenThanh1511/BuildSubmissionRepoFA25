using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountRepository
    {
        private readonly Su25leoparddbContext _context;
        public AccountRepository(Su25leoparddbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> LoginAsync(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

        public async Task<LeopardAccount> LoginAsync(object email, object password)
        {
            throw new NotImplementedException();
        }
    }
}

