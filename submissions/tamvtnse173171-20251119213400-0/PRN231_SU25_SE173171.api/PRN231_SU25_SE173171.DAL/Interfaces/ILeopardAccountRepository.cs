using PRN231_SU25_SE173171.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.DAL.Interfaces
{
    public interface ILeopardAccountRepository
    {
        Task<LeopardAccount> GetUser(string email, string password);
    }
}
