using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173618.api.Models;
using PRN231_SU25_SE173618.api.Models.DTOs;

namespace PRN231_SU25_SE173618.api.Services;

public interface ILeopardProfileService
{
    Task<IEnumerable<LeopardProfileResponseDTO>> GetAllAsync();
    Task<LeopardProfileResponseDTO?> GetByIdAsync(int id);
    Task<LeopardProfileResponseDTO> CreateAsync(CreateLeopardProfileRequest request);
    Task<LeopardProfileResponseDTO?> UpdateAsync(int id, UpdateLeopardProfileRequest request);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<LeopardProfileResponseDTO>> SearchAsync(LeopardProfileSearchRequest request);
}

public class LeopardProfileService : ILeopardProfileService
{
    private readonly Su25leopardDbContext _context;

    public LeopardProfileService(Su25leopardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeopardProfileResponseDTO>> GetAllAsync()
    {
        var profiles = await _context.LeopardProfiles
            .Include(lp => lp.LeopardType)
            .ToListAsync();
        
        return profiles.Select(MapToResponseDTO);
    }

    public async Task<LeopardProfileResponseDTO?> GetByIdAsync(int id)
    {
        var profile = await _context.LeopardProfiles
            .Include(lp => lp.LeopardType)
            .FirstOrDefaultAsync(lp => lp.LeopardProfileId == id);
        
        return profile != null ? MapToResponseDTO(profile) : null;
    }

    public async Task<LeopardProfileResponseDTO> CreateAsync(CreateLeopardProfileRequest request)
    {
        var leopardProfile = new LeopardProfile
        {
            LeopardTypeId = request.LeopardTypeId,
            LeopardName = request.LeopardName,
            Weight = request.Weight,
            Characteristics = request.Characteristics,
            CareNeeds = request.CareNeeds,
            ModifiedDate = DateTime.UtcNow
        };

        _context.LeopardProfiles.Add(leopardProfile);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(leopardProfile.LeopardProfileId) ?? MapToResponseDTO(leopardProfile);
    }

    public async Task<LeopardProfileResponseDTO?> UpdateAsync(int id, UpdateLeopardProfileRequest request)
    {
        var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
        if (leopardProfile == null)
            return null;

        leopardProfile.LeopardTypeId = request.LeopardTypeId;
        leopardProfile.LeopardName = request.LeopardName;
        leopardProfile.Weight = request.Weight;
        leopardProfile.Characteristics = request.Characteristics;
        leopardProfile.CareNeeds = request.CareNeeds;
        leopardProfile.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
        if (leopardProfile == null)
            return false;

        _context.LeopardProfiles.Remove(leopardProfile);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LeopardProfileResponseDTO>> SearchAsync(LeopardProfileSearchRequest request)
    {
        var query = _context.LeopardProfiles
            .Include(lp => lp.LeopardType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.LeopardName))
        {
            query = query.Where(lp => lp.LeopardName.Contains(request.LeopardName));
        }

        if (request.Weight.HasValue)
        {
            query = query.Where(lp => lp.Weight == request.Weight.Value);
        }

        var profiles = await query.ToListAsync();
        return profiles.Select(MapToResponseDTO);
    }

    private static LeopardProfileResponseDTO MapToResponseDTO(LeopardProfile profile)
    {
        return new LeopardProfileResponseDTO
        {
            LeopardProfileId = profile.LeopardProfileId,
            LeopardTypeId = profile.LeopardTypeId,
            LeopardName = profile.LeopardName,
            Weight = profile.Weight,
            Characteristics = profile.Characteristics,
            CareNeeds = profile.CareNeeds,
            ModifiedDate = profile.ModifiedDate,
            LeopardType = profile.LeopardType != null ? new LeopardTypeResponseDTO
            {
                LeopardTypeId = profile.LeopardType.LeopardTypeId,
                LeopardTypeName = profile.LeopardType.LeopardTypeName,
                Origin = profile.LeopardType.Origin,
                Description = profile.LeopardType.Description
            } : null
        };
    }
} 