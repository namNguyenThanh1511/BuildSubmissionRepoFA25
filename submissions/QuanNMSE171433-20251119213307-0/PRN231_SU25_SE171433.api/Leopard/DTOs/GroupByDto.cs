using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard
{
    public class GroupByDto
    {
        public string LeopardType { get; set; }
        public List<LeopardProfile> LeopardProfiles { get; set; }
    }
}
