using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class AccountRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AccountResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
