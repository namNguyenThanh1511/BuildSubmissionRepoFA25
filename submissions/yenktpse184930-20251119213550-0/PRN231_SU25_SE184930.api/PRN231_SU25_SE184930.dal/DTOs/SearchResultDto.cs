using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.DTOs
{
    public class SearchResultDto
    {
        public string LeopardType { get; set; }
        public List<LeopardProfileReponseDto> Leopards { get; set; }
    }
}
