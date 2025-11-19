namespace PRN232_SU23_SE170578.api.Services
{
    public interface IAuthService
    {
        Task<(string Token, string Role)?> Authenticate(string email, string password);
    }
}
