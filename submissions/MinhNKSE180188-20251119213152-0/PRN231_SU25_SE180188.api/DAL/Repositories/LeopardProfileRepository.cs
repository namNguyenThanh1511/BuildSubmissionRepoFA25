using DAL.Models;
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

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).AsNoTracking().FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public IQueryable<LeopardProfile> Search(string? modelName, double? weight)
        {
            var query = _context.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelName))
                query = query.Where(h => h.LeopardName.Contains(modelName));

            if (weight != null)
                query = query.Where(h => h.Weight == weight);

            return query;
        }

        public async Task AddAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
            if (leopardProfile != null)
            {
                _context.LeopardProfiles.Remove(leopardProfile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
