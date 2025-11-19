using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Base;
using Repository.Models;

namespace Service
{
    public interface IAccountService
    {
        Task<LeopardAccount?> Authenticate(string a, string b);
    }   
    public class AccountService: IAccountService
    {
        private readonly AccountRepository _repo;
        public AccountService() => _repo = new AccountRepository();

        public async Task<LeopardAccount?> Authenticate(string a, string b)
        {
            return await _repo.GetAccountAsync(a, b);
        }
    }
}
