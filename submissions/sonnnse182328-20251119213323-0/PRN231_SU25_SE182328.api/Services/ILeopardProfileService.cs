using BusinessObjects;

namespace Services
{
    public interface ILeopardProfileService
    {
        public List<LeopardProfile> GetAllLeopardProfiles();
        public LeopardProfile GetLeopardProfileById(int id);

        public LeopardProfile GetLeopardProfileByName(string modelName);

        public void AddLeopardProfile(LeopardProfile LeopardProfile);
        public void UpdateLeopardProfile(LeopardProfile LeopardProfile);
        public void DeleteLeopardProfile(int id);
    }
}
