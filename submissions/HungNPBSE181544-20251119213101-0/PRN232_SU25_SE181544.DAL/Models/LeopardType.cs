using System;
using System.Collections.Generic;

namespace PRN232_SU25_SE181544.DAL.Models;

public partial class LeopardType
{
    public int LeopardTypeId { get; set; }

    public string? LeopardTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LeopardProfile> LeopardProfiles { get; set; } = new List<LeopardProfile>();
}
