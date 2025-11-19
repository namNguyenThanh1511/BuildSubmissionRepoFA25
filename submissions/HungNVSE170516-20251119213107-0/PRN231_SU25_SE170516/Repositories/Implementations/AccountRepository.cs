using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DBContext _context;

        public AccountRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount?> GetAccountByEmailAsync(string email)
        {
           return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a=>a.Email == email);
        }

        public async Task<LeopardAccount?> GetAccountByIdAsync(int accountId)
        {
                        return await _context.LeopardAccounts
            .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}
