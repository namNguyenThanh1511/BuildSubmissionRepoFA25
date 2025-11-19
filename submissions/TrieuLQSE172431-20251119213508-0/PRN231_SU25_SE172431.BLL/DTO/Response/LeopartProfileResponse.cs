using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.BLL.DTO.Response
{
    public class LeopartProfileResponse
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }
        public string? LeopardTypeName { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

    }
}
