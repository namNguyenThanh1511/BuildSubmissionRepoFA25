using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ILeopardProfileRepository
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        IQueryable<LeopardProfile> Search(string? modelName, double? weight);
        Task AddAsync(LeopardProfile leopardProfile);
        Task UpdateAsync(LeopardProfile leopardProfile);
        Task DeleteAsync(int id);
    }
}
