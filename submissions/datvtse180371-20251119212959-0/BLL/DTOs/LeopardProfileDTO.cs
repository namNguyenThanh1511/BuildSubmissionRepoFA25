using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.DTOs
{
    public class CreateLeopardRequest
    {
        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "LeopardName must start with uppercase letter or number and contain only letters, numbers, and #")]
        public string LeopardName { get; set; } = string.Empty;

        [Range(15, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double? Weight { get; set; }

        public string? Characteristics { get; set; }

        public string? CareNeeds { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int TypeId { get; set; }
    }
    public class UpdateLeopardRequest
    {
        [Required(ErrorMessage = "LeopardName is required")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "LeopardName must start with uppercase letter or number and contain only letters, numbers, and #")]
        public string LeopardName { get; set; } = string.Empty;

        [Range(15, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
        public double? Weight { get; set; }

        public string? Characteristics { get; set; } 

        public string? CareNeeds { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int TypeId { get; set; }

    }

    public class LeopardResponse
    {
        public int LeopardProfileId { get; set; }
        public string LeopardName { get; set; } = string.Empty;

        public double? Weight { get; set; }

        public string? Characteristics { get; set; } = null!;

        public string? CareNeeds { get; set; } = null!;

        public DateTime? ModifiedDate { get; set; }

        public TypeInfo? Type { get; set; }
    }

    public class TypeInfo
    {
        public int LeopardTypeId { get; set; }

        public string? LeopardTypeName { get; set; } = string.Empty;

        public string? Origin { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public static implicit operator TypeInfo?(LeopardType? v)
        {
            throw new NotImplementedException();
        }
    }

    public class ErrorResponse
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
