using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _repository;

        public LeopardAccountService(ILeopardAccountRepository repository)
        {
            _repository = repository;
        }

        public LeopardAccount? Authenticate(string email, string password)
        {
            return _repository.GetActiveAccountByEmailAndPassword(email, password);
        }

        public string? GetRole(LeopardAccount account)
        {
            return account.RoleId.ToString();
        }

        public bool IsTokenAllowed(LeopardAccount account)
        {
            return account.RoleId is 5 or 6 or 7 or 4;
        }
    }
}
