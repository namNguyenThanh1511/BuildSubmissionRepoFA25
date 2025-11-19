using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ILeopardProfileRepository
    {
        IQueryable<LeopardProfile> Query();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task CreateAsync(LeopardProfile handbag);
        Task UpdateAsync(LeopardProfile handbag);
        Task DeleteAsync(LeopardProfile handbag);
        Task<int> GetMaxIdAsync();
    }
}
