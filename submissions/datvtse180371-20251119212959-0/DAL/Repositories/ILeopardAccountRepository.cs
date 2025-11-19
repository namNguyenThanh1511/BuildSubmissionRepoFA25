using DAL.Models;

namespace DAL.Repositories;

public interface ILeopardAccountRepository : IGenericRepository<LeopardAccount>
{
    Task<LeopardAccount?> GetByEmailAsync(string email);
    Task<bool> ValidateCredentialsAsync(string email, string password);
}