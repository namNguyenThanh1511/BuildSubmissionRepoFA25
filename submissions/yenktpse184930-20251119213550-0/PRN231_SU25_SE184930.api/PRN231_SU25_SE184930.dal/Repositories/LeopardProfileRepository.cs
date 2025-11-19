
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184930.dal.DBContext;
using PRN231_SU25_SE184930.dal.Interfaces;
using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Repositories
{
    public  class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles
                .Include(l => l.LeopardType)
                .ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles
                .Include(l => l.LeopardType)
                .FirstOrDefaultAsync(l => l.LeopardTypeId == id);
        }

        public async Task<LeopardProfile> CreateAsync(LeopardProfile leopardProfile)
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(l => (int?)l.LeopardProfileId) ?? 0;
            leopardProfile.LeopardProfileId = maxId + 1;

            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(leopardProfile.LeopardProfileId);
        }

        public async Task<LeopardProfile> UpdateAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(leopardProfile.LeopardProfileId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
            if (leopardProfile == null)
                return false;

            _context.LeopardProfiles.Remove(leopardProfile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LeopardProfile>> SearchAsync(string leopardName, int weight)
        {
            var query = _context.LeopardProfiles.Include(l => l.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(leopardName))
            {
                query = query.Where(l => l.LeopardName.Contains(leopardName));
            }

            if (weight!=null)
            {
                query = query.Where(l => l.Weight.Equals(weight));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LeopardProfiles.AnyAsync(l => l.LeopardProfileId == id);
        }
    }
}
