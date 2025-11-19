using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ILeoPardProfileRepository
    {
        Task<IEnumerable<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task<LeopardProfile> CreateAsync(LeopardProfile profile);
        Task<LeopardProfile> UpdateAsync(LeopardProfile profile);
        Task<bool> DeleteAsync(int id);
    }
}
