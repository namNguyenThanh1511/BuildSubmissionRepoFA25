using Repository.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Base
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Su25leopardDbContext _context;
        public AuthRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public LeopardAccount? GetAccountByEmailAndPassword(string email, string password)
        {
            return _context.LeopardAccounts.FirstOrDefault(a => a.Email == email && a.Password == password);
        }
    }
}
