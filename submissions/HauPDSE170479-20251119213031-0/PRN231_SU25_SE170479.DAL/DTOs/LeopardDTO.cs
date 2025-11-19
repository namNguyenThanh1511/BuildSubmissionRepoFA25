using PRN231_SU25_SE170479.DAL.Models;

namespace PRN231_SU25_SE170479.DAL.DTOs
{
    public record LeopardDTO(string LeopardName, double Weight, string Characteristics, string CareNeeds, int LeopardTypeId);
    public class GetLeopardProfileResponse
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }

        public GetLeopardTypeResponse LeopardType { get; set; }
    }

    public class GetLeopardTypeResponse
    {
        public int LeopardTypeId { get; set; }

        public string LeopardTypeName { get; set; }

        public string Origin { get; set; }

        public string Description { get; set; }

    }
}
