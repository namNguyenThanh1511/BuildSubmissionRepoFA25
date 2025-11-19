using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SU25LeopardDBContext _context;

        public AccountRepository(SU25LeopardDBContext context)
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
