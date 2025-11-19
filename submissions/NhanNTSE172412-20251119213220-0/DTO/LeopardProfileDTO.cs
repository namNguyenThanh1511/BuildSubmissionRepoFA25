using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public class LeopardProfileDTO
    {
        [JsonIgnore]
        public int LeopardProfileId { get; set; }

        public int? LeopardTypeId { get; set; }

        [Required(ErrorMessage = "Leopard Name is required.")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "Leopard Name format is invalid")]
        public string LeopardName { get; set; }

        [Required(ErrorMessage = "Weight  is required.")]
        [Range(16, double.MaxValue, ErrorMessage = "Weight must be > 15")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Characteristics  is required.")]
        public string Characteristics { get; set; }

        [Required(ErrorMessage = "CareNeeds is required.")]
        public string CareNeeds { get; set; }

    }
}
