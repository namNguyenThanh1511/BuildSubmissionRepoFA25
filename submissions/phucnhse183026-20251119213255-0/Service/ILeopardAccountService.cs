using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ILeopardAccountService
    {
        Task<LeopardAccount> Login(string email, string password);
    }
}
