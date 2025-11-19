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
        private readonly ILeopardAccountRepository systemAccountRepository;

        public LeopardAccountService(ILeopardAccountRepository repository)
        {
            this.systemAccountRepository = repository;
        }

        public LeopardAccount Login(string email, string password)
        {
            return systemAccountRepository.Login(email, password);
        }
    }
}
