using PRN232_SU23_SE170578.api.Data;
using PRN232_SU23_SE170578.api.Models;

namespace PRN232_SU23_SE170578.api.Repositories
{
    public class LeopardProfileRepo : ILeopardProfileRepo
    {
        public async Task<LeopardProfile> Add(LeopardProfile profile)
        {
            return await ProfleDAO.Instance.AddProfile(profile);
        }

        public async Task<LeopardProfile> Delete(int id)
        {
            return await ProfleDAO.Instance.Delete(id);
        }

        public async Task<List<LeopardProfile>> GetAllLeopardProfiles()
        {
            return await ProfleDAO.Instance.GetAllProfile();
        }

        public async Task<LeopardProfile> GetOne(int id)
        {
            return await ProfleDAO.Instance.GetById(id);
        }

        public async Task<LeopardProfile> Update(LeopardProfile profile)
        {
            return await ProfleDAO.Instance.Update(profile);
        }
    }
}
