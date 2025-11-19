namespace PRN231_SU25_SE173618.api.Models.DTOs;

public class LeopardTypeResponseDTO
{
    public int LeopardTypeId { get; set; }
    public string? LeopardTypeName { get; set; }
    public string? Origin { get; set; }
    public string? Description { get; set; }
}

public class LeopardProfileResponseDTO
{
    public int LeopardProfileId { get; set; }
    public int LeopardTypeId { get; set; }
    public string LeopardName { get; set; } = null!;
    public double Weight { get; set; }
    public string Characteristics { get; set; } = null!;
    public string CareNeeds { get; set; } = null!;
    public DateTime ModifiedDate { get; set; }
    public LeopardTypeResponseDTO? LeopardType { get; set; }
} 