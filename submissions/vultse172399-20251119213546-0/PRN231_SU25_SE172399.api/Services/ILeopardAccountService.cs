using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardAccountService
    {
        Task<List<LeopardAccount>> GetLeopardAccounts();
        Task<LeopardAccount> GetLeopardAccountById(int id);
        Task<LeopardAccount> Login(string email, string password);
    }
}
