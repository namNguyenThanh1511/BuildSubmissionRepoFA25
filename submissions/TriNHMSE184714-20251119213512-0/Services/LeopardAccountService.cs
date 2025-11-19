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
        private readonly LeopardAccountRepository _repo;
        public LeopardAccountService() => _repo = new LeopardAccountRepository();

        public async Task<LeopardAccount?> Authenticate(string email, string password)
        {
            return await _repo.GetAccountAsync(email, password);
        }
    }
}
