using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class LeopardAccountRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LeopardAccountResponseDTO
    {
        public string Token { get; set; }
        public string RoleId { get; set; }
        public int AccountId { get; set; }
    }
}
