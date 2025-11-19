using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace Leopard.DTOs
{
    public class CreateDTO
    {
        
        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }
        
        public double Weight { get; set; }
        
        public string Characteristics { get; set; }
        
        public string CareNeeds { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

}
