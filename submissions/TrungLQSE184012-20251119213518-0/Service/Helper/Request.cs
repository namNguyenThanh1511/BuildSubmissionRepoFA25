using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper
{
    public class LoginRequestModels
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public record LeopardCreateRequest(

        [Required(ErrorMessage = "LeopardProfileId is required.")]
        [Range(15, int.MaxValue, ErrorMessage = "LeopardProfileId must be greater than 0.")]
            int LeopardProfileId,

        [Required(ErrorMessage = "Profile name is required.")]
        [RegularExpression(
            "^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "Profile name must start with an uppercase letter and can contain alphanumeric characters and spaces."
        )]
            string LeopardName,

        [Required(ErrorMessage = "Weight is required.")]
        [Range(15, double.MaxValue, ErrorMessage = "Price must be greater than 15.")]
            double Weight,

        [Required(ErrorMessage = "Characteristics is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Characteristics must contain only letters.")]
            string Characteristics,

        [Required(ErrorMessage = "CareNeeds is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "CareNeeds must contain only letters.")]
            string CareNeeds,

        [Required(ErrorMessage = "LeopardTypeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "LeopardTypeId must be greater than 0.")]
            int LeopardTypeId,

        [Required(ErrorMessage = "ModifiedDate is required.")]
        DateTime ModifiedDate
    );
    public record LeopardUpdateRequest(

        [Required(ErrorMessage = "LeopardProfileId is required.")]
        [Range(15, int.MaxValue, ErrorMessage = "LeopardProfileId must be greater than 0.")]
            int LeopardProfileId,

        [Required(ErrorMessage = "Profile name is required.")]
        [RegularExpression(
            "^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            ErrorMessage = "Profile name must start with an uppercase letter and can contain alphanumeric characters and spaces."
        )]
            string LeopardName,

        [Required(ErrorMessage = "Weight is required.")]
        [Range(15, double.MaxValue, ErrorMessage = "Price must be greater than 15.")]
            double Weight,

        [Required(ErrorMessage = "Characteristics is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Characteristics must contain only letters.")]
            string Characteristics,

        [Required(ErrorMessage = "CareNeeds is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "CareNeeds must contain only letters.")]
            string CareNeeds,

        [Required(ErrorMessage = "LeopardTypeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "LeopardTypeId must be greater than 0.")]
            int LeopardTypeId,

        [Required(ErrorMessage = "ModifiedDate is required.")]
        DateTime ModifiedDate
    );

    public class ProfileQueryRequest
    {
        public string? LeopardName { get; set; }
        public double? Weight { get; set; }
    }
}
