using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs
{
    public class LeopardProfileFilter
    {
        public string? LeopardName { get; set; }
        public int? weight { get; set; } = 0;
    }
}
