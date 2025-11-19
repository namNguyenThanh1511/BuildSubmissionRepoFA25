using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthRepository
    {
        private readonly SU25LeopardDBContext _context;

        public AuthRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount?> Login(string email, string password)
        {
            return await _context.LeopardAccount
                .Where(x => x.Email == email && x.Password == password)
                .FirstOrDefaultAsync();
        }
    }
}
