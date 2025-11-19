using BusinessObjects;
using DAO;

namespace Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        public void AddLeopardProfileAsync(LeopardProfile LeopardProfile)
            => LeopardProfileDAO.Instance.AddLeopardProfile(LeopardProfile);

        public void DeleteLeopardProfileAsync(int id)
            => LeopardProfileDAO.Instance.DeleteLeopardProfile(id);

        public List<LeopardProfile> GetAllLeopardProfilesAsync()
            => LeopardProfileDAO.Instance.GetAllLeopardProfiles();

        public LeopardProfile GetLeopardProfileByIdAsync(int id)
            => LeopardProfileDAO.Instance.GetLeopardProfileById(id);

        public LeopardProfile GetLeopardProfileByNameAsync(string? name)
            => LeopardProfileDAO.Instance.GetLeopardProfileByName(name);

        public void UpdateLeopardProfileAsync(LeopardProfile LeopardProfile)
            => LeopardProfileDAO.Instance.UpdateLeopardProfile(LeopardProfile.LeopardProfileId, LeopardProfile);
    }
}
