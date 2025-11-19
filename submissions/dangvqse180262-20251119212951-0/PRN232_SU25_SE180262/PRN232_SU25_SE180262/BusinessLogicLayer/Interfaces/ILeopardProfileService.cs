using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfile>> GetAllAsync();

        Task<LeopardProfile?> GetByIdAsync(int id);

        Task<LeopardProfile> CreateAsync(LeopardProfile entity);

        Task<LeopardProfile> UpdateAsync(int id, LeopardProfile entity);

        Task DeleteAsync(int id);

        Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight);
    }
}
