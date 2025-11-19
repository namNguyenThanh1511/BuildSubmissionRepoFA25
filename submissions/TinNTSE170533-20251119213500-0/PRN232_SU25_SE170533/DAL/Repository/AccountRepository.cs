using DAL.Interface;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SU25LeopardDBContext _context;
        public AccountRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }
        public async Task<LeopardAccount?> GetAccountByEmailAsync(string email)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<LeopardAccount?> GetAccountByIdAsync(int accountId)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}
