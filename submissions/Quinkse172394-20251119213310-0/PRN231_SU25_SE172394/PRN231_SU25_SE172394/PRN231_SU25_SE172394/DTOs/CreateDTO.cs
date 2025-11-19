using Repositories.Models;

namespace PRN231_SU25_SE172394.DTOs
{
    public class CreateDTO
    {    

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
     
    }
}
