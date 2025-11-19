using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ILeopardAccountService
    {
        LeopardAccount? Authenticate(string email, string password);
        string? GetRole(LeopardAccount account);
        bool IsTokenAllowed(LeopardAccount account);
    }
}
