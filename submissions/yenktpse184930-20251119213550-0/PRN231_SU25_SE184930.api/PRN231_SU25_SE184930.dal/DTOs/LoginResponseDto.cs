using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int Role { get; set; }
    }
}
