using DAL;
using DAL.Models;

namespace BLL
{
    public class AccountBL : IAccountBL
    {
        private readonly AccountDAO _dao;

        public AccountBL(AccountDAO dao)
        {
            _dao = dao;
        }

        public async Task<LeopardAccount> Login(string email, string password)
        {
            return await _dao.Login(email, password);
        }
    }
}
