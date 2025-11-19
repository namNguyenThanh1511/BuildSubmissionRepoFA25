using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs
{
    public class LeopardTypeDTO
    {
        public int LeopardTypeId { get; set; }

        public string? LeopardTypeName { get; set; }

        public string? Origin { get; set; }

        public string? Description { get; set; }
    }
}
