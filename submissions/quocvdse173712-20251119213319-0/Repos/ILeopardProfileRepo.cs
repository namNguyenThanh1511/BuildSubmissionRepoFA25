using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface ILeopardProfileRepo
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task AddAsync(LeopardProfile t);
        Task UpdateAsync(LeopardProfile t);
        Task DeleteAsync(LeopardProfile t);

    }
}
