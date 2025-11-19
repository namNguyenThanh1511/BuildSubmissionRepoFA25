using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class GroupedHandbagDto
    {
        public string LeoPardTypeName { get; set; } = string.Empty;
        public List<LeoPardModel> LeoPards { get; set; } = new();
    }
}
