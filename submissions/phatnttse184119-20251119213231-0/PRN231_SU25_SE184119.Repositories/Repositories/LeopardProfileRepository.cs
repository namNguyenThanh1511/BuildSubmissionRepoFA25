using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184119.Repositories.IRepositories;
using PRN231_SU25_SE184119.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184119.Repositories.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileRepository(SU25LeopardDBContext context)
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

        public async Task CreateAsync(LeopardProfile LeopardProfile)
        {
            await _context.LeopardProfiles.AddAsync(LeopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Update(LeopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Remove(LeopardProfile);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetMaxIdAsync()
        {
            return await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
        }

    }
}
