using BusinessObjects;

namespace Repositories
{
    public interface ILeopardProfileRepository
    {
        List<LeopardProfile> GetAllLeopardProfilesAsync();
        LeopardProfile GetLeopardProfileByIdAsync(int id);
        LeopardProfile GetLeopardProfileByNameAsync(string? name);
        void AddLeopardProfileAsync(LeopardProfile LeopardProfile);
        void UpdateLeopardProfileAsync(LeopardProfile LeopardProfile);
        void DeleteLeopardProfileAsync(int id);
    }
}
