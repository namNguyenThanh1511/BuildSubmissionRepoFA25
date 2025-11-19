using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{

    public class CreateRequest
    {
       
        [Required]
        public int LeopardTypeId { get; set; }
        [Required]
        public string LeopardName { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Characteristics { get; set; }
        [Required]
        public string CareNeeds { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

    }
    

}
