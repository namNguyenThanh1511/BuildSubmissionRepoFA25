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
    public class LeopardTypeRepo : GenericRepository<LeopardType>
    {
        public LeopardTypeRepo() { }

        public async Task<List<LeopardType>> GetAll()
        {
            var items = await _context.LeopardTypes.ToListAsync();

            return items;
        }
    }
}

