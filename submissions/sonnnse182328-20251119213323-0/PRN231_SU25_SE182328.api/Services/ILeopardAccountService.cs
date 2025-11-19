using BusinessObjects;

namespace Services
{
    public interface ILeopardAccountService
    {
        LeopardAccount GetLeopardAccountByIdAsync(string email, string password);
    }
}
