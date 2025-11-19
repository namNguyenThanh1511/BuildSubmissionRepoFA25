using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.services.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public int Role { get; set; }
    }
}
