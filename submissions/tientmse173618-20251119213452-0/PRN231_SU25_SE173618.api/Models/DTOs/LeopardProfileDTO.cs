using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE173618.api.Models.DTOs;

public class CreateLeopardProfileRequest
{
    public int LeopardProfileId { get; set; }
    
    [Required]
    public int LeopardTypeId { get; set; }
    
    [Required]
    [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*\s)*([A-Z0-9][a-zA-Z0-9]*)$", 
        ErrorMessage = "LeopardName must start with uppercase letter or digit and contain only alphanumeric characters")]
    public string LeopardName { get; set; } = null!;
    
    [Required]
    [Range(16, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
    public double Weight { get; set; }
    
    [Required]
    public string Characteristics { get; set; } = null!;
    
    [Required]
    public string CareNeeds { get; set; } = null!;
}

public class UpdateLeopardProfileRequest
{
    [Required]
    public int LeopardTypeId { get; set; }
    
    [Required]
    [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*\s)*([A-Z0-9][a-zA-Z0-9]*)$", 
        ErrorMessage = "LeopardName must start with uppercase letter or digit and contain only alphanumeric characters")]
    public string LeopardName { get; set; } = null!;
    
    [Required]
    [Range(16, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
    public double Weight { get; set; }
    
    [Required]
    public string Characteristics { get; set; } = null!;
    
    [Required]
    public string CareNeeds { get; set; } = null!;
}

public class LeopardProfileSearchRequest
{
    public string? LeopardName { get; set; }
    public double? Weight { get; set; }
} 