using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class LeopardProfileRepo : GenericRepository<LeopardProfile>
	{
		public LeopardProfileRepo() { }

		public IQueryable<LeopardProfile> GetAllQueryable()
		{
			return _context.LeopardProfiles.Include(x => x.LeopardType);
		}

		public async Task<List<LeopardProfile>> GetAll()
		{
			var items = await _context.LeopardProfiles.Include(x => x.LeopardType).ToListAsync();

			return items;
		}

		public async Task<LeopardProfile> GetByIdAsync(int code)
		{
			var item = await _context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == code);

			return item;
		}

		public async Task<List<LeopardProfile>> Search(string? leopardName, double? weight)
		{

			var query = _context.LeopardProfiles
				.Include(x => x.LeopardType).AsQueryable();

			if (!string.IsNullOrEmpty(leopardName))
				query = query.Where(x => x.LeopardName.Contains(leopardName));
			if (weight.HasValue)
				query = query.Where(x => x.Weight == weight);

			var result = await query.ToListAsync();

			return result;
		}

		public async Task<int> GetNextId()
		{
			var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
			return maxId + 1;
		}
	}
}
