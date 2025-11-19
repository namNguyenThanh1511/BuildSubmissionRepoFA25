using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        public LeopardAccount Login(string email, string password)
        {
            return LeopardAccountDAO.Instance.Login(email, password);
        }
    }
}
