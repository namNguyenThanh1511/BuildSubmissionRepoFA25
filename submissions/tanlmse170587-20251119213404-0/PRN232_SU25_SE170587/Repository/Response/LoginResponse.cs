using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;

        public LoginResponse(string token, string role)
        {
            Token = token;
            Role = role;
        }
    }
}
