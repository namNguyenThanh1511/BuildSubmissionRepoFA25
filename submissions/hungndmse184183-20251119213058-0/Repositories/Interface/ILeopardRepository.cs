using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ILeopardRepository
    {
        Task<IEnumerable<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task<LeopardProfile> AddAsync(LeopardProfile leopardProfile);
        Task<bool> DeleteAsync(int id);
        Task UpdateAsync(LeopardProfile leopardProfile);
    }
}
