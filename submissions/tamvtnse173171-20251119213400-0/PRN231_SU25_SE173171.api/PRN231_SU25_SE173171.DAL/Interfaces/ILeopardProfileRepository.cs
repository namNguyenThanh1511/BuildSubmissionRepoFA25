using PRN231_SU25_SE173171.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.DAL.Interfaces
{
    public interface ILeopardProfileRepository
    {
        Task<List<LeopardProfile>> GetAll();
        Task<LeopardProfile> GetById(int id);
        Task Add(LeopardProfile profile);
        Task Update(LeopardProfile profile);
        Task Delete(int id);
    }
}
