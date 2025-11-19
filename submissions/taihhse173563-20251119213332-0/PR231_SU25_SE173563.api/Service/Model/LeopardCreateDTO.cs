

namespace Service.Model
{
    public class LeopardCreateDTO
    {
        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

    }
}
