using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class AccountRepo:IAccountRepo
    {
        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await AccountDAO.Instance.Login(email, password);
        }

    }
}
