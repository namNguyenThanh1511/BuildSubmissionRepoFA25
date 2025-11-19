using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.DAL
{
    public class SystemAccountRepository : GenericRepository<Model.LeopardAccount>
    {
        private readonly Su25leopardDbContext _context;

        public SystemAccountRepository() => _context ??= new Su25leopardDbContext();

        public async Task<Model.LeopardAccount> LogIn(string email, string password)
        {
            var user = await _context.LeopardAccounts
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password );
            return user;
        }
    }
}
