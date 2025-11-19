using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Su25leopardDbContext _context;

        public AuthRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount?> GetActiveUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
