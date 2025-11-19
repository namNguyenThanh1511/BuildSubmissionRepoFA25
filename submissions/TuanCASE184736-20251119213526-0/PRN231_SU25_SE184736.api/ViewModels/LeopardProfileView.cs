using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE184736.api.ViewModels
{
    public class LeopardProfileView
    {
        [Required]
        public int LeopardProfileId { get; set; }

        [Required]
        public int LeopardTypeId { get; set; }

        [Required]
        [RegularExpression("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$")]
        public string LeopardName { get; set; }

        [Required]
        [Range(minimum: 15, maximum: double.MaxValue, MinimumIsExclusive = true)]
        public double Weight { get; set; }

        [Required]
        public string Characteristics { get; set; }

        [Required]
        public string CareNeeds { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
