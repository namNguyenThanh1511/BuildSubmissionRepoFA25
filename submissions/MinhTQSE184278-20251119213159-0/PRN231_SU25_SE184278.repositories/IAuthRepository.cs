using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.repositories
{
    public interface IAuthRepository
    {
        Task<LeopardAccount?> GetAccountAsync(string email, string password);
    }
}
