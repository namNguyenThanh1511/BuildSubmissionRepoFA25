using DAL.repository;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public interface ILeopardService
    {

        Task AddLeopardProfile(LeopardProfile LeopardProfile);

        Task DeleteLeopardProfile(string id);

        Task<LeopardProfile> GetLeopardProfile(string id);

        Task<List<LeopardProfile>> GetLeopardProfiles();

        Task UpdateLeopardProfile(LeopardProfile LeopardProfile);

    }
}
