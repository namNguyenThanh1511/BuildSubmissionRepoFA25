using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Su25leopardDbContext _context;

        public AccountRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public LeopardAccount GetByEmailAndPassword (string email, string password)
        {
            return _context.LeopardAccounts
                .FirstOrDefault(a => a.Email == email && a.Password == password);
        }
    }
}
