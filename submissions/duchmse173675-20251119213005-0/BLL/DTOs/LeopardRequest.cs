using System.Text.RegularExpressions;

namespace BLL.DTOs
{
    public class LeopardRequest
    {
        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(LeopardName))
                throw new ArgumentException("Leopard name is required.");

            if (string.IsNullOrWhiteSpace(CareNeeds))
                throw new ArgumentException("CareNeeds is required.");

            if (string.IsNullOrWhiteSpace(Characteristics))
                throw new ArgumentException("Characteristics is required.");

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(LeopardName))
                throw new ArgumentException("LeopardName format is invalid.");

            if (Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0.");

            if (LeopardTypeId <= 0)
                throw new ArgumentException("LeopardType Id must be greater than 0.");
        }
    }
}
