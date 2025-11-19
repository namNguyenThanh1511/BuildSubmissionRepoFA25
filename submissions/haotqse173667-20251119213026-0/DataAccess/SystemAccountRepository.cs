using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SystemAccountRepository : ISystemAccountRepository
    {

        private readonly SU25LeopardDBContext _context;

        public SystemAccountRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }
        public async Task<LeopardAccount> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password );
        }
    }
}
