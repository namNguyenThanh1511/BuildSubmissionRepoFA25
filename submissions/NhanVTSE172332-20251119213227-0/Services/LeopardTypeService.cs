using Repositories;
using Repositories.Models;

namespace Services
{
	public interface ILeopardTypeService
	{
		Task<List<LeopardType>> GetAll();
	}
	public class LeopardTypeService : ILeopardTypeService
	{
		private readonly LeopardTypeRepo _repository;

		public LeopardTypeService() => _repository = new LeopardTypeRepo();

		public async Task<List<LeopardType>> GetAll()
		{
			return await _repository.GetAll();
		}
	}
}
