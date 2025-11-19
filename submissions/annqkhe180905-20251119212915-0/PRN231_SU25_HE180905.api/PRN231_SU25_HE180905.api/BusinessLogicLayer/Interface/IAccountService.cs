using DataAccessLayer.ReqAndRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface IAccountService
    {
        public AuthResponse Authenticate(string email, string password);
    }
}
