using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProfileRepository :  GenericRepository<LeopardProfile>, IProfileRepository
    {
        public ProfileRepository(Su25leopardDbContext context) : base(context)
        {
        }
        public async Task<LeopardProfile?> FindById(int id)
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<LeopardProfile> CreateItem(LeopardProfile item)
        {
            var brandExists = await _context.LeopardProfiles.AnyAsync(b => b.LeopardTypeId == item.LeopardTypeId);
            if (!brandExists)
                throw new KeyNotFoundException("Brand does not exist");
            await _context.LeopardProfiles.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<LeopardProfile> UpdateItem(LeopardProfile item)
        {
            var brandExists = await _context.LeopardProfiles.AnyAsync(b => b.LeopardTypeId == item.LeopardTypeId);
            if (!brandExists)
                throw new KeyNotFoundException("Brand does not exist");
            _context.LeopardProfiles.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }


        
    }
}
