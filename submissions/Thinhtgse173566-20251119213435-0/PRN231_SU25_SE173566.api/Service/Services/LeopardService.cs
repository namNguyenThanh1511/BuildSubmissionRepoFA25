using Repository.DTOs;
using Repository.Interface;
using Repository.Models;
using Service.Interface;

namespace Service.Services
{
	public class LeopardService : ILeopardService
	{
		private readonly ILeopardProfileRepo _profileRepo; 
		public LeopardService(ILeopardProfileRepo profileRepo)
		{
			_profileRepo = profileRepo;
		}

		public Task<IEnumerable<LeopardProfile>> GetAllAsync() => _profileRepo.GetAllAsync();

		public Task<LeopardProfile> GetByIdAsync(int id) => _profileRepo.GetByIdAsync(id);

		public Task AddAsync(LeopardProfileCreateDTO entity) => _profileRepo.AddAsync(entity);

		public Task UpdateAsync(LeopardProfile entity) => _profileRepo.UpdateAsync(entity);

		public Task DeleteAsync(int id) => _profileRepo.DeleteAsync(id);

	}
}
