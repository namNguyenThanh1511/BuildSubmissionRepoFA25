namespace Repositories.DTOs
{
    public class LeopardProfileResponseDTO
    {
        public int LeopardProfileId { get; set; }
        public int LeopardTypeId { get; set; }
        public string LeopardName { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Characteristics { get; set; } = string.Empty;
        public string CareNeeds { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
        public LeopardTypeResponseDTO? LeopardType { get; set; }
    }

    public class LeopardTypeResponseDTO
    {
        public int LeopardTypeId { get; set; }
        public string LeopardTypeName { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}