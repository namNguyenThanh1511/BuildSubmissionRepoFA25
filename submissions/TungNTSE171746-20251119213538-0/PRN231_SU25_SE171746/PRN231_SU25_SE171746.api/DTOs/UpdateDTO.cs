using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE171746.api.DTOs
{
    public class UpdateDTO
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

        public DateTime ModifiedDate { get; set; }
    }
}
