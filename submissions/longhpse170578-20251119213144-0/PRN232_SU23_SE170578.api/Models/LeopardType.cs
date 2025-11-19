using System;
using System.Collections.Generic;

namespace PRN232_SU23_SE170578.api.Models
{
    public partial class LeopardType
    {
        public LeopardType()
        {
            LeopardProfiles = new HashSet<LeopardProfile>();
        }

        public int LeopardTypeId { get; set; }
        public string? LeopardTypeName { get; set; }
        public string? Origin { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<LeopardProfile> LeopardProfiles { get; set; }
    }
}
