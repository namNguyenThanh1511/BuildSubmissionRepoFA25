using BusinessObjects;
using Repositories;

namespace Services
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _LeopardAccountRepository;
        public LeopardAccountService(ILeopardAccountRepository LeopardAccountRepository)
        {
            _LeopardAccountRepository = LeopardAccountRepository;
        }

         public LeopardAccount GetLeopardAccountByIdAsync(string email, string password)
        {
            return _LeopardAccountRepository.GetAccount(email, password);
        }
    }
}
