using PRN231_SU25_SE184119.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRN231_SU25_SE184119.Repositories.IRepositories
{
    public interface ILeopardProfileRepository
    {
        IQueryable<LeopardProfile> Query();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task CreateAsync(LeopardProfile LeopardProfile);
        Task UpdateAsync(LeopardProfile LeopardProfile);
        Task DeleteAsync(LeopardProfile LeopardProfile);
        Task<int> GetMaxIdAsync();
    }
}
