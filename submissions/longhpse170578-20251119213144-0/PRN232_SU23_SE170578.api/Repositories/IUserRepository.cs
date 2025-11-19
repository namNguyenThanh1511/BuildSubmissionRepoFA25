using PRN232_SU23_SE170578.api.Models;

namespace PRN232_SU23_SE170578.api.Repositories
{
    public interface IUserRepository
    {
        Task<LeopardAccount> GetUserByEmailAndPassword(string email, string password);
    }
}
