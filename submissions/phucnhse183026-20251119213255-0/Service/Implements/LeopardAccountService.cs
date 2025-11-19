using DAO.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implements
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _repository;

        public LeopardAccountService(ILeopardAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _repository.GetByEmailAndPasswordAsync(email, password);
        }
    }
}
