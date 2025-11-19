using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ILeopardProfileRepository
    {
        Task<IEnumerable<LeopardProfile>> GetAllProfilesAsync();
        Task<LeopardProfile?> GetProfileByIdAsync(int profileId);
        Task<LeopardProfile> CreateProfileAsync(LeopardProfile profile);
        Task<LeopardProfile> UpdateProfileAsync(LeopardProfile profile);
        Task<bool> DeleteProfileAsync(int profileId);
        Task<IEnumerable<LeopardProfile>> SearchProfilesAsync(string? leopardName, double? weight);
    }
}