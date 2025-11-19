using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto
{
    public class LoginRespont
    {
        public string token { get; set; }
        public string role { get; set; }


        public LoginRespont() { }
        public LoginRespont(string token, string role)
        {
            this.token = token;
            this.role = role;
        }
    }
}
