using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AccountDAO
    {
        private readonly SU25LeopardDBContext _context;

        public AccountDAO(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _context.LeopardAccount
                                 .FirstOrDefaultAsync(account =>
                                     account.Email == email &&
                                     account.Password == password);
        }
    }
}
