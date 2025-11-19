using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly LeopardAccountRepository _repository;

        public LeopardAccountService() => _repository = new LeopardAccountRepository();

        public async Task<LeopardAccount> GetLeopardAccountById(int id)
        {
            return await _repository.GetLeopardAccountById(id);
        }

        public async Task<List<LeopardAccount>> GetLeopardAccounts()
        {
            return await _repository.GetLeopardAccounts();
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _repository.Login(email, password);
        }
    }
}
