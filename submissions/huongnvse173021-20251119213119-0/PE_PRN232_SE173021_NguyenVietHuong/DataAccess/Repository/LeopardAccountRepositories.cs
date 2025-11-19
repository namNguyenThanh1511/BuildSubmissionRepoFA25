using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardAccountRepositories
    {
        private readonly Su25leopardDbContext _context;
        public LeopardAccountRepositories(Su25leopardDbContext context)
        {
            _context = context;
        }
        public async Task<LeopardAccount> CheckAccount(string accountEmail, string accountPassword)
        {
            var result = await _context.LeopardAccounts.FirstOrDefaultAsync(n => n.Email.Equals(accountEmail) && n.Password.Equals(accountPassword));
            return result;
        }
    }
}
