using AutoMapper;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardProfileRepositories
    {
        private readonly Su25leopardDbContext _context;
        private readonly IMapper _mapper;
        public LeopardProfileRepositories(Su25leopardDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LeopardProfileDTO>> GetAllLeopardProfileAsync()
        {
            var LeopardProfiles = await _context.LeopardProfiles.Include(c => c.LeopardType).ToListAsync();
            var result = _mapper.Map<List<LeopardProfileDTO>>(LeopardProfiles);
            return result;
        }
        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            var LeopardProfile = await _context.LeopardProfiles.Include(c => c.LeopardType).FirstOrDefaultAsync(c => c.LeopardProfileId == id);

            return LeopardProfile;
        }
        public async Task CreateLeopardProfileAsync(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Add(LeopardProfile);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Update(LeopardProfile);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.LeopardProfiles.FindAsync(id);
            if (entity == null) return false;
            _context.LeopardProfiles.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<LeopardProfileDTO>> SearchAsync(string? modelName, double? weight)
        {
            var query = _context.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelName))
                query = query.Where(h => h.LeopardName.Contains(modelName));

            //if (!string.IsNullOrWhiteSpace(material))
            //    query = query.Where(h => h.Weight.Contains(material));

            var list = await query.ToListAsync();
            return _mapper.Map<List<LeopardProfileDTO>>(list);
        }
    }
}
