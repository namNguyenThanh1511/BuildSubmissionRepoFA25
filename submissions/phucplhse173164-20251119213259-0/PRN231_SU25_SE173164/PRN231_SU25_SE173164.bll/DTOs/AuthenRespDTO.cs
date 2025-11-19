using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173164.bll.DTOs
{
    public class AuthenRespDTO
    {
        public string Token { get; set; } = null!;
        public int? Role { get; set; }
    }
}
