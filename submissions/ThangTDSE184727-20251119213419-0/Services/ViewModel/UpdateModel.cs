using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class UpdateModel
    {

        public int LeopardTypeId { get; set; }
        [Required]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "The leopard name must be in the following format: Aaaa")]
        public string LeopardName { get; set; }
        [Required(ErrorMessage = "Weight is required")]
        [Range(15, int.MaxValue, ErrorMessage = "weight > 15")]
        public double Weight { get; set; }
        [Required]
        public string Characteristics { get; set; }
        [Required]
        public string CareNeeds { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
