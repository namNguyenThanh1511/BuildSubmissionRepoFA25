using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardAccountRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = _context.LeopardAccounts
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }
    }
}
