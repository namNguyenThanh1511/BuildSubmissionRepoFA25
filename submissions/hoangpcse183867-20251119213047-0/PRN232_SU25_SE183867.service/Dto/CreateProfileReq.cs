using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE183867.service.Dto
{
    public class CreateProfileReq
    {
        public int LeopardProfileId { get; set; }
        public int LeopardTypeId { get; set; }
        public string LeopardName { get; set; }
        [Range(16, double.MaxValue, ErrorMessage = "Weight must be greather than 15")]
        public double Weight { get; set; }
        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
