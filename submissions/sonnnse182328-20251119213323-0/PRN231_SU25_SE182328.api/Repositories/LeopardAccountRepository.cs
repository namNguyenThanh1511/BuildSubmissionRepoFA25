using BusinessObjects;
using DAO;

namespace Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        public LeopardAccount GetAccount(string email, string password)
            => LeopardAccountDAO.Instance.GetAccount(email, password);
    }
}
