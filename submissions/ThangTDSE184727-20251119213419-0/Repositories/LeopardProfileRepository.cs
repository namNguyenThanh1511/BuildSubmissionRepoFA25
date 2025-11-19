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
        public LeopardProfileRepository() { }   
        public async Task<int> GetMaxLeopard()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(l => l.LeopardProfileId);
            return maxId;
        }
        public IQueryable<LeopardProfile> GetQueryable()
        {
            return _context.LeopardProfiles.AsQueryable();
        }
    }
}
