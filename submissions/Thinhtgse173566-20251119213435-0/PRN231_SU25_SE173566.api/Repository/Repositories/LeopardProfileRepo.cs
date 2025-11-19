using Microsoft.EntityFrameworkCore;
using Repository.DTOs;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
	public class LeopardProfileRepo : ILeopardProfileRepo
	{

		private readonly SU25LeopardDBContext _context;

		public LeopardProfileRepo(SU25LeopardDBContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
		{
			return await _context.LeopardProfiles.ToListAsync();
		}

		public async Task<LeopardProfile> GetByIdAsync(int id)
		{
			return await _context.LeopardProfiles.FindAsync(id);
		}

		public async Task AddAsync(LeopardProfileCreateDTO entity)
		{
			var newLeopardProfile = new LeopardProfile();
			newLeopardProfile.LeopardProfileId = entity.LeopardProfileId;
			newLeopardProfile.LeopardName = entity.LeopardName;
			newLeopardProfile.CareNeeds = entity.CareNeeds;
			newLeopardProfile.Characteristics = entity.Characteristics;
			newLeopardProfile.Weight = entity.Weight;
			newLeopardProfile.LeopardTypeId = entity.LeopardTypeId;
			newLeopardProfile.ModifiedDate = entity.ModifiedDate;

			_context.LeopardProfiles.Add(newLeopardProfile);
			//await _context.SaveChangesAsync();
			
		}

		public async Task UpdateAsync(LeopardProfile entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.LeopardProfiles.FindAsync(id);
			if (entity != null)
			{
				_context.LeopardProfiles.Remove(entity);
				await _context.SaveChangesAsync();
			}
		}

	
	}
}
