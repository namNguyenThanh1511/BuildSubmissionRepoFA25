using BusinessObjects.Models;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class LeopardAccountRepo : ILeopardAccountRepo
    {
        public async Task<LeopardAccount> LoginAsync(string email, string password)
        {
            return await LeopardAccountDAO.Instance.LoginAsync(email, password);
        }
    }
}
