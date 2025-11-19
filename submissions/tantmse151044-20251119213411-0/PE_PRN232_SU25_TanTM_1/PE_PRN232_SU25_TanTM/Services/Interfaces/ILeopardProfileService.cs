using Repositories.DTOs;

namespace Services.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfileResponseDTO>> GetAllProfilesAsync();
        Task<LeopardProfileResponseDTO?> GetProfileByIdAsync(int profileId);
        Task<LeopardProfileResponseDTO> CreateProfileAsync(LeopardProfileDTO profileDto);
        Task<LeopardProfileResponseDTO> UpdateProfileAsync(int id, LeopardProfileDTO profileDto);
        Task<bool> DeleteProfileAsync(int profileId);
        Task<IEnumerable<LeopardProfileResponseDTO>> SearchProfilesAsync(string? leopardName, double? weight);
    }
}