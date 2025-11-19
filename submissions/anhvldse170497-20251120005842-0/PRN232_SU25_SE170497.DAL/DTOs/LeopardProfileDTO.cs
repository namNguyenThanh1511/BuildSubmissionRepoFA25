using PRN232_SU25_SE170497.DAL.Models;

namespace PRN232_SU25_SE170497.DAL.DTOs
{
    public record LeopardProfileDTO(string LeopardName, string Characteristics, double Weight, string CareNeeds, int LeopardTypeId);
    public class GetLeopardProfileRespone
    {
        public int LeopardProfileId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? LeopardTypeId { get; set; }

        public GetLeopardTypeRespone LeopardType { get; set; }
    }
    public class GetLeopardTypeRespone
    {
        public int LeopardTypeId { get; set; }

        public string LeopardTypeName { get; set; }

        public string Origin { get; set; }

        public string Description { get; set; }

    }
}
