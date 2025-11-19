using Microsoft.EntityFrameworkCore;
using Repositories.Entity;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ObjRepository : IObjRepository
    {
        private readonly LeopardDbContext _context;
        public ObjRepository(LeopardDbContext context) => _context = context;

        public async Task<List<LeopardProfile>> GetAllAsync() =>
            await _context.LeopardProfiles.Include(h => h.LeopardType).ToListAsync();

        public async Task<List<LeopardProfile>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .OrderBy(h => h.LeopardTypeId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id) =>
            await _context.LeopardProfiles.Include(h => h.LeopardType).FirstOrDefaultAsync(h => h.LeopardProfileId == id);

        public async Task AddAsync(LeopardProfile obj)
        {
            _context.LeopardProfiles.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile obj)
        {
            _context.LeopardProfiles.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile obj)
        {
            _context.LeopardProfiles.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeopardProfile>> SearchPagedAsync(string? a, double? b, int pageIndex, int pageSize)
        {
            var query = _context.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(a))
                query = query.Where(h => h.LeopardName.Contains(a));

            if (b != null)
                query = query.Where(h => h.Weight == b);

            return await query
                .OrderBy(h => h.LeopardType!.LeopardTypeName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
