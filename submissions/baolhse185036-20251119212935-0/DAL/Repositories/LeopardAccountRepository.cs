using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardAccountRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public LeopardAccount? GetActiveAccountByEmailAndPassword(string email, string password)
        {
            return _context.LeopardAccounts
                .FirstOrDefault(a => a.Email == email && a.Password == password);
        }
    }
}
