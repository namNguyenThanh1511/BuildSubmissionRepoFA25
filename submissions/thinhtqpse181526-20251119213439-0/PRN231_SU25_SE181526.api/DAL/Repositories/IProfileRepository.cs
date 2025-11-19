using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProfileRepository : IGenericRepository<LeopardProfile>
    {
        Task<LeopardProfile?> FindById(int id);
        Task<LeopardProfile> CreateItem(LeopardProfile item);
        Task<LeopardProfile> UpdateItem(LeopardProfile item);

    }
    
}
