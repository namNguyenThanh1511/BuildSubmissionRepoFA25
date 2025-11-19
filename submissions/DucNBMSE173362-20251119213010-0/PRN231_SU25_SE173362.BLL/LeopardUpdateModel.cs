using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.BLL
{
    public class LeopardUpdateModel
    {
        [Required(ErrorMessage = "LeopardProfileId is required")]
        public int LeopardProfileId { get; set; }

        [Required(ErrorMessage = "LeopardTypeId is required")]
        public int LeopardTypeId { get; set; }

        [RegularExpression("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "Each word must start with a capital letter or digit.")]
        [DefaultValue("")]
        [Required(ErrorMessage = "leopardName is required")]

        public string LeopardName { get; set; } = null!;

        [Required(ErrorMessage = "Weight is required")]
        [Range(15, int.MaxValue, ErrorMessage = "Weight > 15")]
        [DefaultValue(1)]

        public double Weight { get; set; }
        [Required(ErrorMessage = "Characteristics is required")]

        public string Characteristics { get; set; } = null!;
        [Required(ErrorMessage = "CareNeeds is required")]

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; } = DateTime.Now; // Default to current date and time

    }
}
