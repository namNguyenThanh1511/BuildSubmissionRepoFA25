using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class LeopardProfileDTO
    {
        [JsonIgnore]
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        [Required(ErrorMessage = "LeopardName is required.")]
        [StringLength(100, ErrorMessage = "LeopardName  cannot exceed 100 characters.")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "LeopardName format is invalid")]
        public string LeopardName { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be > 0")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Characteristics is required.")]
        public string Characteristics { get; set; }

        [Required(ErrorMessage = "CareNeeds is required.")]
        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
