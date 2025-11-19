using DataAccess.Entities;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class UserService
    {
        private readonly LeopardAccountRepositories _repo;
        public UserService(LeopardAccountRepositories repo)
        {
            _repo = repo;
        }
        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _repo.CheckAccount(email, password);
        }
    }
}
