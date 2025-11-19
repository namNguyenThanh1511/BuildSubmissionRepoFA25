using Microsoft.EntityFrameworkCore;
using PRN232_SU23_SE170578.api.Data;
using PRN232_SU23_SE170578.api.Models;

namespace PRN232_SU23_SE170578.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount> GetUserByEmailAndPassword(string email, string password)
        {
            return await _context.LeopardAccounts.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
