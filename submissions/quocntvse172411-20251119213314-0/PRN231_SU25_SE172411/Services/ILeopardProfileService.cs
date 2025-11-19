using BusinessObjects;
using DataTransferObjects;

namespace Services
{
    public interface ILeopardProfileService
    {
        void AddLeopardProfile(LeopardProfileDTO item);
        List<LeopardProfile> GetLeopardProfile();
        LeopardProfile GetLeopardProfileById(int id);
        void RemoveLeopardProfile(int id);
        void UpdateLeopardProfile(UpdateLeopardProfileDTO item);
    }
}