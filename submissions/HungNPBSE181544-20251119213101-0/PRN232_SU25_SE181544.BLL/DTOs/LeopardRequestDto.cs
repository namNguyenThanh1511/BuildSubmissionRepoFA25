using PRN232_SU25_SE181544.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE181544.BLL.DTOs
{
    public class LeopardRequestDto
    {
        [Required]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "modelName format is invalid")]
        public int LeopardProfileId { get; set; }
        [Required]
        public int LeopardTypeId { get; set; }
        [Required]

        public string LeopardName { get; set; }
        [Required]
        [Range(15, double.MaxValue, ErrorMessage = "weight must be greater than 15")]

        public double Weight { get; set; }
        [Required]

        public string Characteristics { get; set; }
        [Required]

        public string CareNeeds { get; set; }
        [Required]

        public DateTime ModifiedDate { get; set; }
    }
}
