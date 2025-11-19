using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SU25Leopard.BusinessObject.DTO
{
    public class LeopardProfileRequestDTO
    {
        [Required]
        public int LeopardTypeId { get; set; }
        [Required]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "LeopardName format is invalid")]
        public string LeopardName { get; set; }
        [Required]
        [Range(15, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }
        [Required]
        public string Characteristics { get; set; }
        [Required]
        public string CareNeeds { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
