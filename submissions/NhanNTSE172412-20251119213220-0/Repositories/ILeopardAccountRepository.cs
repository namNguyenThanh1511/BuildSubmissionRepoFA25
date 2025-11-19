using BusinessObjects;

namespace Repositories
{
    public interface ILeopardAccountRepository
    {
        LeopardAccount Login(string email, string password);
    }
}