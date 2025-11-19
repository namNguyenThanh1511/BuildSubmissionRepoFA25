using PRN231_SU25_SE184119.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184119.Services.IServices
{
    public interface ILeopardAccountService
    {
        Task<LeopardAccount?> LoginRequest(string username, string password);
    }
}
