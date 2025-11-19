using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Dtos 
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public int Role { get; set; } 
    }
}
