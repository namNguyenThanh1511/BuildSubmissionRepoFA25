using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Su25leopardDbContext _context;
        public AuthRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public Task<LeopardAccount?> GetAccountAsync(string email, string password)
        {
            return _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
