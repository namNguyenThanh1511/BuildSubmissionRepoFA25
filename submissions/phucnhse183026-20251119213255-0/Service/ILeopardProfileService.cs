using DAO.DTO;
using DAO.Models;

namespace Service
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task CreateAsync(CreateProfile a);
        Task UpdateAsync(LeopardProfile a);
        Task DeleteAsync(int a);
    }
}
