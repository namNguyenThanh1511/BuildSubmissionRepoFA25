using PRN232_SU23_SE170578.api.Models;
using PRN232_SU23_SE170578.api.Repositories;

namespace PRN232_SU23_SE170578.api.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepo _repository;
        public LeopardProfileService(ILeopardProfileRepo repository)
        {
            _repository = repository;
        }
        public async Task<LeopardProfile> Add(LeopardProfile profile)
        {
            return await _repository.Add(profile);
        }

        public async Task<LeopardProfile> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            return await _repository.GetAllLeopardProfiles();
        }

        public async Task<LeopardProfile> GetOne(int id)
        {
            return await _repository.GetOne(id);
        }

        public async Task<LeopardProfile> Update(LeopardProfile profile)
        {
            return await _repository.Update(profile); 
        }
    }
}
