using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper
{
    public record ErrorModel
    {
        public string? Message { get; init; }
        public string ErrorCode { get; init; }

        public ErrorModel(string? message, string errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }

    public class ProfileResponse
    {
        public int LeopardProfileId { get; set; }

        public int LeopardTypeId { get; set; }

        public string LeopardName { get; set; }
        public string LeopardTypeName { get; set; }

        public double Weight { get; set; }

        public string Characteristics { get; set; }

        public string CareNeeds { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
