using PRN231_SU25_SE181580.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE181580.BLL.DTOs {
    public class LeopardProfileDTO {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

    }


}
