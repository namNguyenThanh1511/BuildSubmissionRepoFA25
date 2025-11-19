using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LoginRequestDTO
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
