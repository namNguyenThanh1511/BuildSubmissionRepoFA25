using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class LeopardType
{
    [Key]
    public int LeopardTypeId { get; set; }

    public string? LeopardTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LeopardProfile> LeopardProfiles { get; set; } = new List<LeopardProfile>();
}
