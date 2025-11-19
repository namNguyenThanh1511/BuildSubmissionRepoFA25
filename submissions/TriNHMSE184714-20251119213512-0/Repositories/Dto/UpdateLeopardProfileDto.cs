using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Dto
{
    public class UpdateLeopardProfileDto
    {
        [Required]
        public int LeopardProfileId { get; set; }
        [Required]

        public int LeopardTypeId { get; set; }
        [Required]

        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*)(\s+[A-Z0-9][a-zA-Z0-9]*)*$", ErrorMessage = "Each word in this field must begin with a capital letter or a number, and special characters (#, @, &, (, )) are not allowed.")]
        public string LeopardName { get; set; } = string.Empty;
        [Required]

        [Range(15, 1000000, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }
        [Required]

        public string Characteristics { get; set; } = string.Empty;
        [Required]

        public string CareNeeds { get; set; } = string.Empty;
        [Required]

        public DateTime ModifiedDate { get; set; }
    }
}
