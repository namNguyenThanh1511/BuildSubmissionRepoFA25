using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173175.Service.DTOs
{
	public class LeopardProfileRequest
	{
		[Required(ErrorMessage = "LeopardProfileId is required")]
		public int LeopardProfileId { get; set; }

		[Required(ErrorMessage = "LeopardTypeId is required")]
		public int LeopardTypeId { get; set; }

		[Required(ErrorMessage = "LeopardName is required")]
		[RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
			ErrorMessage = "LeopardName must start with uppercase letter or number, and can contain letters, numbers, #, and spaces")]
		public string LeopardName { get; set; } = null!;

		[Required(ErrorMessage = "LeopardTypeId is required")]
		[Range(15.01, double.MaxValue, ErrorMessage = "Price must be a decimal > 15")]
		public double Weight { get; set; }

		public string Characteristics { get; set; } = null!;

		public string CareNeeds { get; set; } = null!;
		public DateTime ModifiedDate { get; set; }
	}

	public class LeopardProfileUpdateRequest
	{
		[Required(ErrorMessage = "LeopardTypeId is required")]
		public int LeopardTypeId { get; set; }

		[Required(ErrorMessage = "LeopardName is required")]
		[RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
			ErrorMessage = "LeopardName must start with uppercase letter or number, and can contain letters, numbers, #, and spaces")]
		public string LeopardName { get; set; } = null!;

		[Required(ErrorMessage = "LeopardTypeId is required")]
		[Range(15.01, double.MaxValue, ErrorMessage = "Price must be a decimal > 15")]
		public double Weight { get; set; }

		public string Characteristics { get; set; } = null!;

		public string CareNeeds { get; set; } = null!;
	}


	public class LeopardSearchRequest
	{
		public string? LeopardName { get; set; }
		public double? Weight { get; set; }
	}
}
