using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ILeopardAccountService
    {
        Task<List<LeopardAccount>> GetAllAsync();
        Task<LeopardAccount?> GetByIdAsync(int id);
        Task<LeopardAccount> CreateAsync(LeopardAccount leopardAccount);
        Task<bool> DeleteAsync(int id);
        Task<LeopardAccount?> Login(string email, string password);
    }
}
