using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface ILeopardProfileRepository
    {
        Task<List<LeopardProfile>> GetAllProfilesAsync();
        Task<LeopardProfile> GetProfileByIdAsync(int id);
        Task CreateProfileAsync(LeopardProfile profile);
        Task UpdateProfileAsync(LeopardProfile profile);
        Task DeleteProfileAsync(int id);
        Task<List<LeopardProfile>> SearchProfilesAsync(string leopardName, string cheetahName, double? weight);
    }
}
