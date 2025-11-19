using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; } = null!;
    }


    public class ProfileRequestDto
    {
        public int LeopardProfileId { get; set; }

        [Required(ErrorMessage = "LeopardTypeId is required.")]
        public int LeopardTypeId { get; set; }

        [Required(ErrorMessage = "LeopardName is required.")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "Can only contain letters, numbers, and #, and must start with a capital letter or number.")]
        public string LeopardName { get; set; } = null!;

        [Required(ErrorMessage = "Weight is required.")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Characteristics is required.")]
        public string Characteristics { get; set; } = null!;
        [Required(ErrorMessage = "CareNeeds is required.")]
        public string CareNeeds { get; set; } = null!;
        [Required(ErrorMessage = "ModifiedDate is required.")]
        public DateTime ModifiedDate { get; set; }

    }
}
