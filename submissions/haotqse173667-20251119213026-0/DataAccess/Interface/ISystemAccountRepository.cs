using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface ISystemAccountRepository
    {
        Task<LeopardAccount> GetByEmailAndPasswordAsync(string email, string password);
    }
}
