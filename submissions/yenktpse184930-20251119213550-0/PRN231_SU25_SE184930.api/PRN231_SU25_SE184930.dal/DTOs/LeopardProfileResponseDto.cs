using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.DTOs
{
    public class LeopardProfileReponseDto
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }

        public LeopardTypeResponseDto LeopardType { get; set; }
    }
}
