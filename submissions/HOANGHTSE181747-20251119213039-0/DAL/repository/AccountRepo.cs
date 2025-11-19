using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repository
{
    public class AccountRepo : IAccountRepo
    {

        private readonly Su25leopardDbContext _context = new Su25leopardDbContext();


        public async Task<LeopardAccount> Login(string email, string password)
        {
            var account = await _context.LeopardAccounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
            return account;
        }
    }
}
