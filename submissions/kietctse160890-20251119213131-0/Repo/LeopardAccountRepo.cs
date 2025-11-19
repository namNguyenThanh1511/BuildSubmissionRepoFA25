using BO;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class LeopardAccountRepo : ILeopardAccountRepo
    {
        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await LeopardAccountDAO.Instance.Login(email, password);
        }
    }
}
