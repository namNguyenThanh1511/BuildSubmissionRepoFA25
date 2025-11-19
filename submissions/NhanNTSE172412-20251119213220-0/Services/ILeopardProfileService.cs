using BusinessObjects;
using DTO;

namespace Services
{
    public interface ILeopardProfileService
    {
        void CreateLeopardProfile(LeopardProfileDTO item);
        LeopardProfile GetLeopardProfileById(int id);
        List<LeopardProfile> GetLeopardProfiles();
        void RemoveLeopardProfile(int id);
        List<object> SearchLeopardProfiles(string? LeopardName, double? Weight);
        void UpdateLeopardProfile(LeopardProfileDTO item);
    }
}