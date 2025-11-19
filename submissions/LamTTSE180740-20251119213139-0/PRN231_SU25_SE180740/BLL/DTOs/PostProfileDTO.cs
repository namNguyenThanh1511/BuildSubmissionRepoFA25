using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class PostProfileDTO
    {
        [Required]
        public int LeopardProfileId { get; set; }
        [Required]
        public int LeopardTypeId { get; set; }
        [Required]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
        ErrorMessage = "Name must start with uppercase letter or number")]
        public string LeopardName { get; set; } = null!;
        [Required]
        [Range(15.01, double.MaxValue, ErrorMessage = "Price must be greater than 15")]
        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
