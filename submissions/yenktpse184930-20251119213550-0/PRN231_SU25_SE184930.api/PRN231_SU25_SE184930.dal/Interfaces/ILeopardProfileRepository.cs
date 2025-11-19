using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Interfaces
{
    public interface ILeopardProfileRepository
    {
        Task<IEnumerable<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task<LeopardProfile> CreateAsync(LeopardProfile leopardProfile);
        Task<LeopardProfile> UpdateAsync(LeopardProfile leopardProfile);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<LeopardProfile>> SearchAsync(string leopardName, int weight);
        Task<bool> ExistsAsync(int id);
    }
}
