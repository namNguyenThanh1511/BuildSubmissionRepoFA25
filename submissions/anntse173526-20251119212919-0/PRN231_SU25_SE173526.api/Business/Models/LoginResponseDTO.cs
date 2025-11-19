using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int Role { get; set; }
    }
}
