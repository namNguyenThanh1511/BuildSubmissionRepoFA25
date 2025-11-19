using DAO.DTO;
using DAO.Models;

namespace Repository
{
    public interface ILeopardProfileRepository
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task CreateAsync(LeopardProfile a);
        Task UpdateAsync(LeopardProfile a);
        Task DeleteAsync(int a);
    }
}
