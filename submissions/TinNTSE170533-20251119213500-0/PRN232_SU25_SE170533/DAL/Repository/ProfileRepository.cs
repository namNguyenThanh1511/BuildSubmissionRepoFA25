using DAL.DTOs;
using DAL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SU25LeopardDBContext _context;
        public ProfileRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task AddProfileAsync(LeopardProfile profile)
        {
            var existProfile = await _context.LeopardProfiles.FindAsync(profile.LeopardProfileId);
            if (existProfile != null)
            {
                throw new BadHttpRequestException("Profile already existed.");
            }
            _context.LeopardProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(int id)
        {
            var profile = await GetProfileByIdAsync(id);

            _context.LeopardProfiles.Remove(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllProfilesAsync()
        {
            var profiles = await _context.LeopardProfiles.Include(hb => hb.LeopardType).AsNoTracking().ToListAsync();
            return profiles;
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {id} not found.");
            }
            return profile;
        }

        public async Task<IEnumerable<ProfileDTOs>> SearchByNameAndWeight(string? name, double? weight)
        {
            var query = _context.LeopardProfiles
                                .Include(h => h.LeopardType)
                                .AsNoTracking()
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(h => h.LeopardName.Contains(name));
            }

            if (weight > 0)
            {
                query = query.Where(h => h.Weight == weight);
            }

            var result = await query
                .Select(g => new ProfileDTOs
                {
                    LeopardProfileId = g.LeopardProfileId,
                    LeopardTypeId = g.LeopardTypeId,
                    LeopardName = g.LeopardName,
                    Weight = g.Weight,
                    Characteristics = g.Characteristics,
                    CareNeeds = g.CareNeeds,
                    ModifiedDate = g.ModifiedDate
                }).ToListAsync();

            return result;
        }

        public async Task UpdateProfileAsync(LeopardProfile profile)
        {
            var existingProfile = await GetProfileByIdAsync(profile.LeopardProfileId);

            var type = await _context.LeopardTypes.FindAsync(profile.LeopardTypeId);
            if (type == null)
            {
                throw new BadHttpRequestException($"Type with ID {profile.LeopardTypeId} not found.");
            }

            _context.Entry(existingProfile).CurrentValues.SetValues(profile);
            _context.LeopardProfiles.Update(existingProfile);
            await _context.SaveChangesAsync();
        }
    }
}
