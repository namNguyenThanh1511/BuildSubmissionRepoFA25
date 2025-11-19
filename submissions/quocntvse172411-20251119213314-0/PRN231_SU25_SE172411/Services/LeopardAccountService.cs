using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _repository;

        public LeopardAccountService(ILeopardAccountRepository repository)
        {
            this._repository = repository;
        }

        public LeopardAccount Login(string email, string password)
        {
            return _repository.Login(email, password);
        }
    }
}
