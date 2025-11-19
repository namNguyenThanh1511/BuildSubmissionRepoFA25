using PRN231_SU25_SE184119.Repositories.IRepositories;
using PRN231_SU25_SE184119.Repositories.Models;
using PRN231_SU25_SE184119.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184119.Services.Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _leopardAccount;
        public LeopardAccountService(ILeopardAccountRepository leopardAccount) { _leopardAccount = leopardAccount; }
        public async Task<LeopardAccount?> LoginRequest(string username, string password)
        {
            return await _leopardAccount.LoginRequest(username, password);
        }

    }
}
