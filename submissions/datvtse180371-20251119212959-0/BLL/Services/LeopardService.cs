using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class LeopardService : ILeopardService
{
    private readonly ILeopardRepository _leopardRepository;

    public LeopardService(ILeopardRepository leopardRepository)
    {
        _leopardRepository = leopardRepository;
    }

    public async Task<IEnumerable<LeopardResponse>> GetAllLeopardsAsync()
    {
        var leopard = await _leopardRepository.GetAllWithTypeAsync();
        return leopard.Select(MapToResponse);
    }

    public async Task<LeopardResponse?> GetLeopardByIdAsync(int id)
    {
        var leopard = await _leopardRepository.GetByIdWithTypeAsync(id);
        return leopard != null ? MapToResponse(leopard) : null;
    }

    public async Task<LeopardResponse> CreateLeopardAsync(CreateLeopardRequest request)
    {
        // Get the next available ID
        var existingProducts = await _leopardRepository.GetAllAsync();
        var nextId = existingProducts.Any() ? existingProducts.Max(h => h.LeopardProfileId) + 1 : 1;

        var leopard = new LeopardProfile
        {
            LeopardProfileId = nextId,
            LeopardName = request.LeopardName,
            Weight = (double)request.Weight,
            Characteristics = request.Characteristics,
            CareNeeds = request.CareNeeds,
            LeopardTypeId = request.TypeId,
            ModifiedDate = DateTime.Today
        };

        await _leopardRepository.AddAsync(leopard);

        // Get the created product with category info
        var createdLeopard = await _leopardRepository.GetByIdWithTypeAsync(leopard.LeopardProfileId);
        return MapToResponse(createdLeopard!);
    }

    public async Task<LeopardResponse?> UpdateLeopardAsync(int id, UpdateLeopardRequest request)
    {
        var leopard = await _leopardRepository.GetByIdAsync(id);
        if (leopard == null)
            return null;

        leopard.LeopardName = request.LeopardName;
        leopard.Weight = (double)request.Weight;
        leopard.Characteristics = request.Characteristics;
        leopard.CareNeeds = request.CareNeeds;
        leopard.LeopardTypeId = request.TypeId;

        _leopardRepository.Update(leopard);

        var updatedLeopard = await _leopardRepository.GetByIdWithTypeAsync(id);
        return MapToResponse(updatedLeopard!);
    }

    public async Task<bool> DeleteLeopardAsync(int id)
    {
        var leopard = await _leopardRepository.GetByIdAsync(id);
        if (leopard == null)
            return false;

        _leopardRepository.Delete(leopard);
        return true;
    }

    public async Task<IEnumerable<IGrouping<string, LeopardResponse>>> SearchLeopardsAsync(string? leopardName, double? weight)
    {
        var groupedLeopard = await _leopardRepository.SearchGroupedByTypeAsync(leopardName, weight);
        return groupedLeopard.Select(group =>
            new Grouping<string, LeopardResponse>(
                group.Key,
                group.Select(MapToResponse)
            ));
    }

    private static LeopardResponse MapToResponse(LeopardProfile leopard)
    {
        return new LeopardResponse
        {
            LeopardProfileId = leopard.LeopardProfileId,
            LeopardName = leopard.LeopardName,
            Weight = leopard.Weight,
            Characteristics = leopard.Characteristics,
            CareNeeds = leopard.CareNeeds,
            ModifiedDate = leopard.ModifiedDate,
            Type = leopard.LeopardType != null ? new LeopardType
            {
                LeopardTypeId = leopard.LeopardType.LeopardTypeId,
                LeopardTypeName = leopard.LeopardType.LeopardTypeName,
                Origin = leopard.LeopardType.Origin,
                Description = leopard.LeopardType.Description
            } : null
        };
    }
}

//public class TypeInfo
//{
//    public int LeopardTypeId { get; set; }

//    public string? LeopardTypeName { get; set; } = string.Empty;

//    public string? Origin { get; set; } = string.Empty;

//    public string? Description { get; set; } = string.Empty;
//}
// Helper class for grouping
public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
{
    private readonly IEnumerable<TElement> _elements;

    public Grouping(TKey key, IEnumerable<TElement> elements)
    {
        Key = key;
        _elements = elements;
    }

    public TKey Key { get; }

    public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}