using BusinessObjects;
using DTO;

namespace Repositories
{
    public interface ILeopardProfileRepository
    {
        void AddLeopardProfile(LeopardProfileDTO item);
        void DeleteLeopardProfile(int id);
        LeopardProfile GetLeopardProfileById(int id);
        List<LeopardProfile> GetLeopardProfiles();
        List<object> SearchLeopardProfiles(string? LeopardName, double? Weight);
        void UpdateLeopardProfile(LeopardProfileDTO item);
    }
}