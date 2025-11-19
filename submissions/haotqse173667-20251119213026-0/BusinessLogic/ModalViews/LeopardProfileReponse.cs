using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ModalViews
{
    public class LeopardProfileReponse
    {
        [Key]
        public int LeopardProfileId { get; set; }
        public string LeopardName { get; set; }
      
        public double Weight { get; set; }
        public string Characteristics { get; set; }
        public string CareNeeds { get; set; }
        public int TypeId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
