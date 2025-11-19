using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LeopardProfileRepository: GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepository()
        {
            
        }

        public new async Task<List<LeopardProfile>?> GetAllInclude()
        {
            var itemList = await _context.LeopardProfiles
                .Include(x => x.LeopardType)
                .ToListAsync();

            return itemList;

        }
        public async Task<LeopardProfile?> GetByIdAsync2(int id)
        {
            var item = await _context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == id);

            return item;

        }
    }
}
