using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class AuthResponse
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
