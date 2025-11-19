using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SystemAccountDAO
    {
        private readonly SU25LeopardDBContext _context;

        public SystemAccountDAO(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _context.LeopardAccounts
                                 .FirstOrDefaultAsync(account =>
                                     account.Email == email &&
                                     account.Password == password);
        }
    }
}
