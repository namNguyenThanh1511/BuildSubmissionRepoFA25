using BLL.DTOs;
using DAL.Entities;

namespace BLL.Services
{

    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetLeopardProfiles();
        Task<LeopardProfile> GetLeopardProfileById(int id);
        Task<LeopardProfile> CreateLeopardProfile(LeopardProfileDTO dto);
        Task<bool> UpdateLeopardProfile(int id, LeopardProfileUpdateDTO dto);
        Task<bool> DeleteLeopardProfile(int id);
        Task<List<LeopardProfile>> SearchLeopardProfile(string leopardName, double weight);
    }


}
