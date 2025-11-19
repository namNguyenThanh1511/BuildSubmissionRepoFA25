using BOs;
using STIService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ILeopardAccountRepository : IGenericRepository<LeopardAccount>
    {
        Task<LeopardAccount> Login(string email, string password);
    }
}
