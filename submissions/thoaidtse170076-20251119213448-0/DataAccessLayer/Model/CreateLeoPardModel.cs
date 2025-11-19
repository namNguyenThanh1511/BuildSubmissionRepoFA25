using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class CreateLeoPardModel
    {
  
        [Required]

        public int LeopardTypeId { get; set; }
        [Required]

        public string LeopardName { get; set; } = null!;
        [Required]

        public double Weight { get; set; }
        [Required]

        public string Characteristics { get; set; } = null!;
        [Required]

        public string CareNeeds { get; set; } = null!;
    }
}
