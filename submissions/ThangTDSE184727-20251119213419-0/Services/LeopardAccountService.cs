using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardAccountService
    {
        private readonly LeopardAccountRepository _leopardAccountRepository;

        public LeopardAccountService()
        {
            _leopardAccountRepository = new LeopardAccountRepository();
        }
        public async Task<LeopardAccount> Authenticate(string email, string password)
        {
            return await _leopardAccountRepository.GetLeopardAccount(email, password);
        }
    }
}
