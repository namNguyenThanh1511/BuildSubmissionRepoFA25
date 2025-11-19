using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetLeopardProfiles();
        Task<LeopardProfile> GetLeopardProfile(int id);


        Task<LeopardProfile> AddLeopardProfile(LeopardProfile leopardProfile);
        Task<LeopardProfile> UpdateLeopardProfile(LeopardProfile leopardProfile);
        Task<LeopardProfile> DeleteLeopardProfile(int id);
    }
}
