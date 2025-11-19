using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173282.BLL
{
    public class LeopardCreateModel
    {
        [RegularExpression("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "Each word must start with a capital letter or digit.")]
        [DefaultValue("")]
        [Required(ErrorMessage = "LeopardName is required")]
        public string? LeopardName { get; set; }

        public string Characteristics { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(16.0, int.MaxValue, ErrorMessage = "Weight > 15")]
        [DefaultValue(16)]
        public double Weight { get; set; }

        [Required(ErrorMessage = "CareNeeds is required")]
        public string? CareNeeds { get; set; }

        [Required(ErrorMessage = "LeopardTypeId is required")]
        public int LeopardTypeId { get; set; }
    }
}
