using System;
using System.Collections.Generic;

namespace SU25Leopard.BusinessObject.Models;

public partial class LeopardProfile
{
    public int LeopardProfileId { get; set; }

    public int LeopardTypeId { get; set; }

    public string LeopardName { get; set; }

    public double Weight { get; set; }

    public string Characteristics { get; set; }

    public string CareNeeds { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual LeopardType LeopardType { get; set; }
}
