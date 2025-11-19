using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231_SU25_SE173164.dal.Entities;

public partial class LeopardType
{
    public int LeopardTypeId { get; set; }

    public string? LeopardTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<LeopardProfile> LeopardProfiles { get; set; } = new List<LeopardProfile>();
}
