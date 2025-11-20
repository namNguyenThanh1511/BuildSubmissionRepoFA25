using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ILeopardProfileRepository : IGenericRepository<LeopardProfile>
    {

        Task<IEnumerable<LeopardProfile>> GetAllWithTypesAsync();
        Task<LeopardProfile?> GetByIdWithTypesAsync(int id);
        Task<IEnumerable<LeopardProfile>> SearchLeopardProfileAsync(string? LeopardName, double? Weight);
    }
}
