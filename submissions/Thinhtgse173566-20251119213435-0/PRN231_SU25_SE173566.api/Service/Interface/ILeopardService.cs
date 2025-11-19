using Repository.DTOs;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface ILeopardService
	{
		Task<IEnumerable<LeopardProfile>> GetAllAsync();
		Task<LeopardProfile> GetByIdAsync(int id);
		Task AddAsync(LeopardProfileCreateDTO entity);
		Task UpdateAsync(LeopardProfile entity);
		Task DeleteAsync(int id);
	}
}
