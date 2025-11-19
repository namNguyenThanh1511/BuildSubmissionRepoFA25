using BusinessObjects;
using DataTransferObjects;

namespace Repositories
{
    public interface ILeopardProfileRepository
    {
        void AddLeopardProfile(LeopardProfileDTO item);
        void DeleteLeopardProfile(int id);
        List<LeopardProfile> GetLeopardProfile();
        LeopardProfile GetLeopardProfileById(int id);
        void UpdateLeopardProfile(UpdateLeopardProfileDTO item);
    }
}