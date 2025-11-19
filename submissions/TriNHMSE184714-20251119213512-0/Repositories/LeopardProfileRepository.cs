using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepository : GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepository()
        {
            
        }
        public new async Task<List<LeopardProfile>?> GetAll()
        {
            var itemList = await _context.LeopardProfiles
                .Include(x => x.LeopardType).ToListAsync();

            return itemList;
        }

    }
}
