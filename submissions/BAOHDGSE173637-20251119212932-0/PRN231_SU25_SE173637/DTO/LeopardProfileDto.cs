using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173637.DTO
{
    public class LeopardProfileDto
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
