using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173282.BLL
{
    public class LeopardUpdateModel
    {
        public int LeopardProfileId { get; set; }

        [Required(ErrorMessage = "LeopardProfileId is required")]
        public int LeopardTypeId { get; set; }

        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "Each word must start with a capital letter or digit.")]
        [DefaultValue("")]
        public string LeopardName { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(16.0, int.MaxValue, ErrorMessage = "Weight > 15")]
        [DefaultValue(16)]
        public double Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
