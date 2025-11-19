using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface ILeopardProfileRepository
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task AddAsync(LeopardProfile leopardProfile);
        Task UpdateAsync(LeopardProfile leopardProfile);
        Task DeleteAsync(LeopardProfile leopardProfile);
        Task<List<LeopardProfile>> SearchAsync(string modelName);
    }
}
