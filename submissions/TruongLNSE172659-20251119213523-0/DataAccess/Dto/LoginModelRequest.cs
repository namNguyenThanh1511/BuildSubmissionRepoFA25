using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto
{
    public class LoginModelRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
