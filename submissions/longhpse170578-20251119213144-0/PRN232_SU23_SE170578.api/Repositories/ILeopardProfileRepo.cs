using PRN232_SU23_SE170578.api.Models;

namespace PRN232_SU23_SE170578.api.Repositories
{
    public interface ILeopardProfileRepo
    {
        Task<List<LeopardProfile>> GetAllLeopardProfiles();
        Task<LeopardProfile> GetOne(int id);
        Task<LeopardProfile> Add(LeopardProfile profile);
        Task<LeopardProfile> Update(LeopardProfile profile);
        Task<LeopardProfile> Delete(int id);
    }
}
