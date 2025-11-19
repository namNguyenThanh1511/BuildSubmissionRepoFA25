using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DAO
{
    public class LeopardAccountDAO
    {
        private readonly Su25leopardDbContext _context;

        public LeopardAccountDAO(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> AuthenticateAsync(string email, string password)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }

}
