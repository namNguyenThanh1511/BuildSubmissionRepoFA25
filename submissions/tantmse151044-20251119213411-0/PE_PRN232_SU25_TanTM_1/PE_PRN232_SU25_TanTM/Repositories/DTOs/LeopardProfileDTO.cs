using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs
{
    public class LeopardProfileDTO
    {
        public int LeopardProfileId { get; set; }

        [Required(ErrorMessage = "LeopardTypeId is required")]
        public int LeopardTypeId { get; set; }

        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*\s)*([A-Z0-9][a-zA-Z0-9]*)$",
            ErrorMessage = "LeopardName must start with uppercase letter or digit, followed by alphanumeric characters")]
        public string LeopardName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Weight is required")]
        [Range(15.01, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Characteristics is required")]
        public string Characteristics { get; set; } = string.Empty;

        [Required(ErrorMessage = "CareNeeds is required")]
        public string CareNeeds { get; set; } = string.Empty;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}