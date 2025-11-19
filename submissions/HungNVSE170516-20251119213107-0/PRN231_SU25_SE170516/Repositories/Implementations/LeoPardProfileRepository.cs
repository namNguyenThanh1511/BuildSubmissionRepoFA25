using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class LeoPardProfileRepository : ILeoPardProfileRepository
    {
        private readonly DBContext _context;

        public LeoPardProfileRepository(DBContext context)
        {
            _context = context;
        }

        public Task<LeopardProfile> CreateAsync(LeopardProfile profile)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile == null)
                return false;

            _context.LeopardProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(b => b.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(b => b.LeopardType).FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public Task<LeopardProfile> UpdateAsync(LeopardProfile profile)
        {
            throw new NotImplementedException();
        }
    }
}
