using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE173635.api.Models.DTOs;

public class CreateLeopardProfileRequest
{
    public int LeopardProfileId { get; set; }

    [Required]
    public int LeopardTypeId { get; set; }

    [Required]
    [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9]*)$", 
        ErrorMessage = "LeopardName must follow the specified format")]
    public string LeopardName { get; set; } = string.Empty;

    [Required]
    [Range(16, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
    public double Weight { get; set; }

    [Required]
    public string Characteristics { get; set; } = string.Empty;

    [Required]
    public string CareNeeds { get; set; } = string.Empty;
}

public class UpdateLeopardProfileRequest
{
    [Required]
    public int LeopardTypeId { get; set; }

    [Required]
    [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9]*)$", 
        ErrorMessage = "LeopardName must follow the specified format")]
    public string LeopardName { get; set; } = string.Empty;

    [Required]
    [Range(16, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
    public double Weight { get; set; }

    [Required]
    public string Characteristics { get; set; } = string.Empty;

    [Required]
    public string CareNeeds { get; set; } = string.Empty;
}

public class LeopardProfileResponse
{
    public int LeopardProfileId { get; set; }
    public int LeopardTypeId { get; set; }
    public string LeopardName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string Characteristics { get; set; } = string.Empty;
    public string CareNeeds { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
    public string LeopardTypeName { get; set; } = string.Empty;
} 