using Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IObjRepository
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile?> GetByIdAsync(int id);
        Task AddAsync(LeopardProfile obj);
        Task UpdateAsync(LeopardProfile obj);
        Task DeleteAsync(LeopardProfile obj);
        Task<List<LeopardProfile>> GetPagedAsync(int pageIndex, int pageSize);
        Task<List<LeopardProfile>> SearchPagedAsync(string? a, double? b, int pageIndex, int pageSize);

    }
}
