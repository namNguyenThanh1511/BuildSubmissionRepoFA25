using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.DTOs
{
    public class UpdateLeopardProfileRequest
    {
        [Required(ErrorMessage = "LeopardTypeId is required")]
        public int LeopardTypeId { get; set; }
        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(
        @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
        ErrorMessage = "LeopardName must start with uppercase or digit, and contain only alphanumeric characters or #")]
        public string LeopardName { get; set; } = null!;
        [Required(ErrorMessage = "Weight is required")]
        [Range(15.01, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Characteristics is required")]
        public string Characteristics { get; set; } = null!;
        [Required(ErrorMessage = "CareNeeds is required")]
        public string CareNeeds { get; set; } = null!;
        [Required(ErrorMessage = "ModifiedDate is required")]
        public DateTime ModifiedDate { get; set; }
    }
}
