using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelView
{
    public class LeopardRequestDTO
    {

        //public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }
    }
}
