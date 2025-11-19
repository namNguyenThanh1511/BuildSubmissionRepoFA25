using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.DTOs
{
    public class LeopardProfileRequestDto
    {
        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public int LeopardTypeId { get; set; }
    }
}
