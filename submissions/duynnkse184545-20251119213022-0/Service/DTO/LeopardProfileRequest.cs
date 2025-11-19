using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Validation;

namespace Service.DTO
{
    public class LeopardProfileRequest
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        [NameValidation]
        public string LeopardName { get; set; }
        [Range(15, double.MaxValue, ErrorMessage = "weight must be greater than 15")]
        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
