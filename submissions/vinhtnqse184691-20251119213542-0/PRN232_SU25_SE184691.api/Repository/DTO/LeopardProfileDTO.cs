using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class LeopardProfileCreate
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"
            , ErrorMessage = "Name must start with capital, and have no special but the pound symbol")]
        public string LeopardName { get; set; }

        [Range(15, double.MaxValue, ErrorMessage = "Weight must be more than 15")]
        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }

    public class LeopardProfileUpdate
    {
        public int? LeopardTypeId { get; set; }

        public string? LeopardName { get; set; }

        public double? Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }

        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
    }

    public class LeopardProfileView
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
