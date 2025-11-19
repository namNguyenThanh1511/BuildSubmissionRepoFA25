using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccService
    {
        private readonly AccRepository _repo;
        public AccService()
        {
            _repo = new AccRepository();
        }

        public async Task<LeopardAccount?> Authenticate(string email, string password)
        {
            var accounts = await _repo.GetAllAsync();
            var account = accounts.FirstOrDefault(x => 
                x.Email.Equals(email.Trim()) &&
                x.Password.Equals(password.Trim())
            );

            return account;
        }
    }
}
