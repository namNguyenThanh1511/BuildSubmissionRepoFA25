using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LepPardProfileRepository : GenericRepository<LeopardProfile>, ILeoPardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LepPardProfileRepository(SU25LeopardDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllWithTypeAsync()
        {
            Console.WriteLine("📦 Repository: Getting all handbags with brand info");
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .ToListAsync();
        }

        public async Task<LeopardProfile?> GetByIdWithTypeAsync(int id)
        {
            Console.WriteLine($"📦 Repository: Getting LeopardId ID {id} with type info");
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }     

        public async Task<int> GetNextIdAsync()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
            return maxId + 1;
        }
    }
}
