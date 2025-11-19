using BusinessObjects;
using Repositories;

namespace Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _LeopardProfileRepository;
        public LeopardProfileService(ILeopardProfileRepository LeopardProfileRepository)
        {
            _LeopardProfileRepository = LeopardProfileRepository;
        }
        public List<LeopardProfile> GetAllLeopardProfiles()
        {
            return _LeopardProfileRepository.GetAllLeopardProfilesAsync();
        }
        public LeopardProfile GetLeopardProfileById(int id)
        {
            return _LeopardProfileRepository.GetLeopardProfileByIdAsync(id);
        }

        public LeopardProfile GetLeopardProfileByName(string modelName)
        {
            return _LeopardProfileRepository.GetLeopardProfileByNameAsync(modelName);
        }

        public void AddLeopardProfile(LeopardProfile LeopardProfile)
        {
            _LeopardProfileRepository.AddLeopardProfileAsync(LeopardProfile);
        }
        public void UpdateLeopardProfile(LeopardProfile LeopardProfile)
        {
            _LeopardProfileRepository.UpdateLeopardProfileAsync(LeopardProfile);
        }
        public void DeleteLeopardProfile(int id)
        {
            _LeopardProfileRepository.DeleteLeopardProfileAsync(id);
        }
    }
}
