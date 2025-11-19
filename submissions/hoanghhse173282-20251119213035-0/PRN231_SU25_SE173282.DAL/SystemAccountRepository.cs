using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.DAL
{
    public class SystemAccountRepository : GenericRepository<LeopardAccount>
    {
        private readonly Su25leopardDbContext _context;

        public SystemAccountRepository() => _context ??= new Su25leopardDbContext();

        public async Task<LeopardAccount> LogIn(string email, string password)
        {
            var user = await _context.LeopardAccounts
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}
