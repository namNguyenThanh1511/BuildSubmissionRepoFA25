using DAL;
using DAL.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _repo;
        public AccountService(IAccountRepo repo)
        {
            _repo = repo;
        }
        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _repo.Login(email, password);
        }
    }
}
