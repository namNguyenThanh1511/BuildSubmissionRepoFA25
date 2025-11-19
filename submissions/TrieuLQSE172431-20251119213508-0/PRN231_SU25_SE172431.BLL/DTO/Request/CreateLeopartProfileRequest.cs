using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE172431.BLL.DTO.Request
{
    public class CreateLeopartProfileRequest
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        [RegularExpression("^([A-Z0-9][a-zA-Z0-9#\\s]*)$")]

        public string LeopardName { get; set; } = null!;

        [Range(15 , double.MaxValue, ErrorMessage = "Weight must be greater than or equal to 15 kg.")]
        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
