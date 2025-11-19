using System.ComponentModel.DataAnnotations;

namespace BEAPI.DTOs
{
    public class UpdateDto
    {
        public int? LeopardTypeId { get; set; }

        public string? LeopardName { get; set; }

        public double? Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
