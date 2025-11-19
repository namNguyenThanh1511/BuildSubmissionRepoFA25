using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IAccountRepo
    {
        Task<LeopardAccount> Login(string email, string password);
    }
}
