using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService
    {
        private readonly LeopardAccountRepo _repository;

        public AuthService() => _repository = new LeopardAccountRepo();

        public async Task<LeopardAccount> Authenticate(string username, string password)
        {
            return await _repository.GetUserAccountAsync(username, password);
        }
    }
}
