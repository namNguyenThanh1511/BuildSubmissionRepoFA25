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
        private readonly LeopardAccountRepo _repository;

        public LeopardAccountService() => _repository = new LeopardAccountRepo();

        public async Task<LeopardAccount> Authenticate(string username, string password)
        {
            return await _repository.GetUserAccountAsync(username, password);
        }
    }
}
