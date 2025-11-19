using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Interfaces
{
    public interface ILeopardAccountRepository
    {
        Task<LeopardAccount> GetByEmailAsync(string email);
        Task<LeopardAccount> GetByIdAsync(int id);
    }
}
