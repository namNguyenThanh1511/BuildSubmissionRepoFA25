using Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<LeopardAccount?> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}
