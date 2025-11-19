using DAL;
using DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Service
{
    public class AccountService
    {
        private readonly IGenericRepo<LeopardAccount> _repo;
        public AccountService(IGenericRepo<LeopardAccount> repo)
        {
            _repo = repo;
        }
        public Task<LeopardAccount> Login(string username, string password)
        {
            var accounts = _repo.GetSingle(a => a.Email == username && a.Password == password);
            return Task.FromResult(accounts);
        }
    }
}
