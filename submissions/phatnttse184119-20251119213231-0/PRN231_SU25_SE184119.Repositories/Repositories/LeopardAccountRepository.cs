using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184119.Repositories.IRepositories;
using PRN231_SU25_SE184119.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184119.Repositories.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly SU25LeopardDBContext _context;
        public LeopardAccountRepository(SU25LeopardDBContext context) { _context = context; }
        public async Task<LeopardAccount?> LoginRequest(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

    }
}
