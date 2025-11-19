using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class LeopardProfileDTO
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

        public string LeopardTypeName { get; set; } = null!;
    }
    public class LeopardProfileCreateDTO
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }
        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "LeopardName is invalid format(Each word must start with a letter or digit, may contain letters, digits, or #, separated by single space)")]
        public string LeopardName { get; set; } = null!;
        [Range(15.0, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
    }
    public class LeopardProfileUpdateDTO
    {
        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
    }
}
