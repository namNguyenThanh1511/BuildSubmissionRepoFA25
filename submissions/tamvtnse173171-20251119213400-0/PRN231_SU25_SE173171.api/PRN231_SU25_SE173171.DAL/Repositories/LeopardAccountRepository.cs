using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173171.DAL.Entities;
using PRN231_SU25_SE173171.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.DAL.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardAccountRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> GetUser(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(s => s.Email == email && s.Password == password);
        }
    }
}
