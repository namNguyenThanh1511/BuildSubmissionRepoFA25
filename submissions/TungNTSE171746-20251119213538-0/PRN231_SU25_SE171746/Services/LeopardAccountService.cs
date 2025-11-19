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
        private readonly LeopardAccountRepository _repository;

        public LeopardAccountService()
        {
            _repository = new LeopardAccountRepository();
        }

        public async Task<LeopardAccount> AuthenticateAsync(string email, string password)
        {
            return await _repository.GetLeopardAccountAsync(email, password);
        }

    }
}
