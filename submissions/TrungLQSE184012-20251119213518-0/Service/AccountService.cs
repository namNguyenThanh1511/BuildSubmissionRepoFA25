using Repo.Base;
using Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService
    {
        private readonly GenericRepository<LeopardAccount> _repo;

        public AccountService(GenericRepository<LeopardAccount> repo)
        {
            _repo = repo;
        }

        public async Task<LeopardAccount?> GetSystemAccountAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;
            return await _repo.GetOneAsync(x =>
                x.Email.Equals(email)
                && x.Password.Equals(password)
            );
        }
    }
}
