using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class LeopardProfileRepo:ILeopardProfileRepo
    {
        public async Task<LeopardProfile> AddLeopardProfile(LeopardProfile leopardProfile)
        {
            return await LeopardProfileDAO.Instance.AddLeopardProfile(leopardProfile);
        }

        public async Task<LeopardProfile> DeleteLeopardProfile(int id)
        {
            return await LeopardProfileDAO.Instance.DeleteLeopardProfile(id);
        }

        public async Task<LeopardProfile> GetLeopardProfile(int id)
        {
            return await LeopardProfileDAO.Instance.GetLeopardProfileById(id);
        }

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            return await LeopardProfileDAO.Instance.GetLeopardProfiles();
        }

        public async Task<LeopardProfile> UpdateLeopardProfile(LeopardProfile leopardProfile)
        {
            return await LeopardProfileDAO.Instance.UpdateLeopardProfile(leopardProfile);
        }
    }
}
