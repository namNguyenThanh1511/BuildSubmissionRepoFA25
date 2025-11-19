namespace PRN231_SU25_SE171746.api.DTOs
{
    public class LeopardProfileDTO
    {
        public int LeopardProfileId { get; set; }


        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
        public LeopardTypeDTO LeopardType { get; set; }

    }
}
