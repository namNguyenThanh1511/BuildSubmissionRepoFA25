using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTOs
{
    public class LeopardProfileRequest
    {
        [Required(ErrorMessage = "LeopardProfileId is required")]
        public int LeopardProfileId { get; set; }
        [Required(ErrorMessage = "LeopardTypeId is required")]
        public int LeopardTypeId { get; set; }

        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)", ErrorMessage = "LeopardName format is invalid")]
        public string LeopardName { get; set; } = null!;

        [Range(15, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }

        public string? Characteristics { get; set; } = null!;

        public string? CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; } 
        
    }
}
