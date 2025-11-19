using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IProfileRepository
    {
        Task AddProfileAsync(LeopardProfile profile);
        Task DeleteProfileAsync(int id);
        Task<IEnumerable<LeopardProfile>> GetAllProfilesAsync();
        Task<LeopardProfile> GetProfileByIdAsync(int id);
        Task<IEnumerable<ProfileDTOs>> SearchByNameAndWeight(string? name, double? weight);
        Task UpdateProfileAsync(LeopardProfile profile);
    }
}
