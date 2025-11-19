using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<LeopardAccount?> GetAccountByEmailAsync(string email);
        Task<LeopardAccount?> GetAccountByIdAsync(int accountId);
    }
}
