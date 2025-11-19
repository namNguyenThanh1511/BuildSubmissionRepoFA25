using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly SU25LeopardDBDbContext _context;

        public LeopardProfileRepository(SU25LeopardDBDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllProfilesAsync()
        {
            return await _context.LeopardProfiles
                .Include(p => p.LeopardType)
                .ToListAsync();
        }

        public async Task<LeopardProfile?> GetProfileByIdAsync(int profileId)
        {
            return await _context.LeopardProfiles
                .Include(p => p.LeopardType)
                .FirstOrDefaultAsync(p => p.LeopardProfileId == profileId);
        }

        public async Task<LeopardProfile> CreateProfileAsync(LeopardProfile profile)
        {
            profile.ModifiedDate = DateTime.Now;
            _context.LeopardProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<LeopardProfile> UpdateProfileAsync(LeopardProfile profile)
        {
            profile.ModifiedDate = DateTime.Now;
            _context.LeopardProfiles.Update(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<bool> DeleteProfileAsync(int profileId)
        {
            var profile = await _context.LeopardProfiles
                .FirstOrDefaultAsync(p => p.LeopardProfileId == profileId);

            if (profile != null)
            {
                _context.LeopardProfiles.Remove(profile);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<LeopardProfile>> SearchProfilesAsync(string? leopardName, double? weight)
        {
            var query = _context.LeopardProfiles.Include(p => p.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(leopardName))
            {
                query = query.Where(p => p.LeopardName.Contains(leopardName));
            }

            if (weight.HasValue)
            {
                query = query.Where(p => p.Weight == weight.Value);
            }

            return await query.ToListAsync();
        }
    }
}