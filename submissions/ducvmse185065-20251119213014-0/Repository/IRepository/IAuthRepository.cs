using Repository.Models;

namespace Repository.IRepository
{
    public interface IAuthRepository
    {
        LeopardAccount? GetAccountByEmailAndPassword(string email, string password);
    }
}
