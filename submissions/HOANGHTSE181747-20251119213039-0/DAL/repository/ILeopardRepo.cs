using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repository
{
    public interface ILeopardRepo
    {

        Task<List<LeopardProfile>> GetLeopardProfiles();
        Task<LeopardProfile> GetLeopardProfile(string id);
        Task AddLeopardProfile(LeopardProfile LeopardProfile);
        Task UpdateLeopardProfile(LeopardProfile LeopardProfile);
        Task DeleteLeopardProfile(string id);
    }
}
