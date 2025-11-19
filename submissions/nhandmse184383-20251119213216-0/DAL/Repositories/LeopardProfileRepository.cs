using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public IQueryable<LeopardProfile> Query()
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType)
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task CreateAsync(LeopardProfile leopardProfile)
        {
            await _context.LeopardProfiles.AddAsync(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Remove(leopardProfile);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetMaxIdAsync()
        {
            return await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
        }

    }
}
