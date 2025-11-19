using BusinessObjects;

namespace Services
{
    public interface ILeopardAccountService
    {
        LeopardAccount Login(string email, string password);
    }
}