using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardProfileRepository : GenericRepository<LeopardProfile>, ILeopardProfileRepository
    {

        private readonly Su25leopardDbContext _leopardDbContext;

        public LeopardProfileRepository(Su25leopardDbContext context) : base(context)
        {
            _leopardDbContext = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllWithTypesAsync()
        {
            return await _leopardDbContext.LeopardProfiles
                .Include(h => h.LeopardType)
                .ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdWithTypesAsync(int id)
        {
            return await _leopardDbContext.LeopardProfiles
                .Include(h => h.LeopardType)
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<IEnumerable<LeopardProfile>> SearchLeopardProfileAsync(string? LeopardName, double? Weight)
        {
            var query = _leopardDbContext.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(LeopardName))
            {
                query = query.Where(h => h.LeopardName.Contains(LeopardName));
            }
            if (Weight != null)
            {
                query = query.Where(h => h.Weight != 0 && h.Weight==Weight);
            }
            return await query.ToListAsync();
        }
    }
}
