using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly Su25leopardDbContext _ctx;
        public LeopardProfileRepository(Su25leopardDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
        {
            return await _ctx.LeopardProfiles
                             .Include(p => p.LeopardType)
                             .ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdAsync(int id)
        {
            return await _ctx.LeopardProfiles
                             .Include(p => p.LeopardType)
                             .FirstOrDefaultAsync(p => p.LeopardProfileId == id);
        }

        public async Task<LeopardProfile> AddAsync(LeopardProfile entity)
        {
            _ctx.LeopardProfiles.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(LeopardProfile entity)
        {
            _ctx.LeopardProfiles.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _ctx.LeopardProfiles.FindAsync(id);
            if (existing != null)
            {
                _ctx.LeopardProfiles.Remove(existing);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight)
        {
            var q = _ctx.LeopardProfiles
                        .Include(p => p.LeopardType)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(leopardName))
                q = q.Where(p => p.LeopardName.Contains(leopardName));

            if (weight.HasValue)
                q = q.Where(p => p.Weight == weight.Value);

            return await q.ToListAsync();
        }
    }

}