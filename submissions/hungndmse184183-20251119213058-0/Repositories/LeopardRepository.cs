using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardRepository : ILeopardRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType)
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<LeopardProfile> AddAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();
            return leopardProfile;
        }       
        public async Task<bool> DeleteAsync(int id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
            if (leopardProfile == null) return false;
            _context.LeopardProfiles.Remove(leopardProfile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeopardProfile>> SearchAsync(string? LeopardName, double? weight)
        {
            var query = _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(LeopardName))
                query = query.Where(h => h.LeopardName.Contains(LeopardName));

            if (weight > 0)
                query = query.Where(h => h.Weight.Equals(weight));

            return query;
        }
    }
}
