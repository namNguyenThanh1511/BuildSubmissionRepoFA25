using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Interfaces
{
    public interface ILeopardTypeRepository
    {
        Task<LeopardType> GetByIdAsync(int id);
        Task<IEnumerable<LeopardType>> GetAllAsync();
        Task<bool> ExistsAsync(int id);
    }
}
