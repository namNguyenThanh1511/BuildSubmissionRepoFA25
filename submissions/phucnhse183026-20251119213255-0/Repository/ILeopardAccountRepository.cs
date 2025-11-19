
using DAO.Models;

namespace Repository
{
    public interface ILeopardAccountRepository
    {
        Task<LeopardAccount> GetByEmailAndPasswordAsync(string email, string password);
    }
}
