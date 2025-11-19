using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE173282.DAL;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.BLL
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
