using PRN231_SU25_SE173362.DAL;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.BLL
{
    public class SystemAccountService
    {
        private readonly SystemAccountRepository _repository;

        public SystemAccountService() => _repository ??= new SystemAccountRepository();

        public async Task<LeopardAccount> LogIn(string username, string password)
        {
            return await _repository.LogIn(username, password);
        }
    }
}
