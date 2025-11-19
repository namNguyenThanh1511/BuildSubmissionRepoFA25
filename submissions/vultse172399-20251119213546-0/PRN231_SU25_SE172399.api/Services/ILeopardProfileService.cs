using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<int> CreateAsync(LeopardProfileDTO leopard);
        Task<int> UpdateAsync(LeopardProfileDTO leopard);
        Task<List<LeopardProfile>> SearchAsync(string name, double? weight);
    }
}
