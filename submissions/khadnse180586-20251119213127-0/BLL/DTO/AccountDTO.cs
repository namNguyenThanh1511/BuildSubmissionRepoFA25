using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class AccountRequestDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;


    }

    public class AccountResponsedDTO
    {
        public string Token { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
