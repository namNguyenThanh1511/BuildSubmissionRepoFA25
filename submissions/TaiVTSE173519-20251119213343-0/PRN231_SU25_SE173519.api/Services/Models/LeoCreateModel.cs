using Services.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Models
{
    public class LeoCreateModel
    {

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; } = null!;

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(LeopardName))
                throw new AppException("HB40001", "modelName is required");

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(LeopardName))
                throw new AppException("HB40001", "modelName is invalid");

            if (Weight <= 15)
                throw new AppException("HB40001", "Weight must be greater than 15");
        }
    }
}
