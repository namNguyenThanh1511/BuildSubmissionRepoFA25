using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LeopardProfileRepo : GenericRepo<LeopardProfile>, ILeopardProfileRepo
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepo(Su25leopardDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LeopardProfile> GetLeopardProfileById(int id)
        {
            return await _context.LeopardProfiles.Include(lp => lp.LeopardType)
                .FirstOrDefaultAsync(lp => lp.LeopardProfileId == id);

        }

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            return await _context.LeopardProfiles
                    .Include(lp => lp.LeopardType)
                    .ToListAsync();
        }

        public async Task<List<LeopardProfile>> SearchLeopard(string leopardName, double weight)
        {
            var query = _context.LeopardProfiles.AsQueryable();

            if (string.IsNullOrWhiteSpace(leopardName))
            {
                query = query.Where(lp => lp.LeopardName.ToLower() == leopardName.ToLower());
            }
            if (weight > 0)
            {
                query = query.Where(lp => lp.Weight.Equals(weight));
            }
            return await query.ToListAsync();
        }
    }
}
