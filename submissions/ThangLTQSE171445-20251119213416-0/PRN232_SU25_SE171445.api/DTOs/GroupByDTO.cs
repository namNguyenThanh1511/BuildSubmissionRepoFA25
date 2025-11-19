using Repositories.Models;

namespace PRN232_SU25_SE171445.api.DTOs
{
    public class GroupByDTO
    {
        public string LeopardType { get; set; }
        public List<LeopardProfile> LeopardProfiles { get; set; }
    }
}
