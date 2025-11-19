using BusinessObjects;

namespace Repositories
{
    public interface ILeopardAccountRepository
    {
        public LeopardAccount GetAccount(string email, string password);
    }
}
