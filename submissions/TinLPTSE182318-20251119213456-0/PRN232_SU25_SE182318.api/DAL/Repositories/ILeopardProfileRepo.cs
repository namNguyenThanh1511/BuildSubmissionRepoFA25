using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ILeopardProfileRepo
    {
        Task<List<LeopardProfile>> SearchLeopard(string leopardName, double weight);
        Task<List<LeopardProfile>> GetLeopardProfiles();
        Task<LeopardProfile> GetLeopardProfileById(int id);
    }
}
