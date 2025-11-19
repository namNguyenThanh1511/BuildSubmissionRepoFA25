using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class LeopardProfileModifyDTO
    {
        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
