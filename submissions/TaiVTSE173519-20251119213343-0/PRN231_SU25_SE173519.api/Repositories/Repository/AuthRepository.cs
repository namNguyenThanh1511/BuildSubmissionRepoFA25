using Microsoft.EntityFrameworkCore;
using Repositories.Entity;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LeopardDbContext _context;
        public AuthRepository(LeopardDbContext context) => _context = context;


        public async Task<LeopardAccount?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
        }
    }
}
