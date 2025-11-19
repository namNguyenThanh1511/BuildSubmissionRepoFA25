using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.repositories
{
    public class LeopardProfileRepository : ILeopardProfileReposiotry
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public IQueryable<LeopardProfile> Query()
        {
            return _context.LeopardProfiles.AsQueryable();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task CreateAsync(LeopardProfile l)
        {
            await _context.LeopardProfiles.AddAsync(l);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile l)
        {
            _context.LeopardProfiles.Update(l);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile l)
        {
            _context.LeopardProfiles.Remove(l);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetMaxIdAsync()
        {
            return await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
        }
    }
}
