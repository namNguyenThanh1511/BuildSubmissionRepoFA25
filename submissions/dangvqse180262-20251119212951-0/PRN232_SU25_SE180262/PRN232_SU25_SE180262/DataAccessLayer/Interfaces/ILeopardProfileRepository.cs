using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ILeopardProfileRepository
    {
        Task<IEnumerable<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task<LeopardProfile> AddAsync(LeopardProfile entity);
        Task UpdateAsync(LeopardProfile entity);
        Task DeleteAsync(int id);

        Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight);
    }
}
