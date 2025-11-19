using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ModalViews
{
    public class LeopardProfileRequest
    {
        public string LeopardName { get; set; }
        public string Characteristic { get; set; }
        
        public string CareNeeds { get; set; }
        public double Weight { get; set; }
        public int TypeId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
