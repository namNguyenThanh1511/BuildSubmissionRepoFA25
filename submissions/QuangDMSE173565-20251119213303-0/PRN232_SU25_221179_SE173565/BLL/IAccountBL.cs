using DAL.Models;

namespace BLL
{
    public interface IAccountBL
    {
        Task<LeopardAccount> Login(string email, string password);
    }
}
