using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LeopardAccountBL : ILeopardAccountBL
    {
        private readonly LeopardAccountDAO _dao;

        public LeopardAccountBL(LeopardAccountDAO dao)
        {
            _dao = dao;
        }
        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _dao.Login(email, password);
        }
    }
}
