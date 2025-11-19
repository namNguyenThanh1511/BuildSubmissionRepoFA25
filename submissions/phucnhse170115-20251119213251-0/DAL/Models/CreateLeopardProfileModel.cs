using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DAL.Entities;

namespace Model
{
    public class CreateLeopardProfileModel
    {
        [Required]
        public int LeopardProfileId { get; set; }
        [Required]
        public int LeopardTypeId { get; set; }
        [Required]
        public string LeopardName { get; set; } = null!;
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Characteristics { get; set; } = null!;
        [Required]
        public string CareNeeds { get; set; } = null!;
        [Required]
        public DateTime ModifiedDate { get; set; }
    }

}

