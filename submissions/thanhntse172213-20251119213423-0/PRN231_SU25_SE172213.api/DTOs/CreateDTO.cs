using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE172213.api.DTOs
{
    public class CreateDTO
    {
        public int? LeopardProfileId { get; set; }

        public int? LeopardTypeId { get; set; }

        public string? LeopardName { get; set; }

        public double? Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

}
