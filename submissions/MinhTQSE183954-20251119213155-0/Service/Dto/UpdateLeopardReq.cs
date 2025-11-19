using System.ComponentModel.DataAnnotations;

namespace Service.Dto;

public class UpdateLeopardReq
{
	public int LeopardTypeId { get; set; }
	[Required]
	[RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$")]
	public string LeopardName { get; set; }
	[Range(15.00, double.MaxValue, ErrorMessage = "Weight must be greater than 15")]
	public double Weight { get; set; }
	public string Characteristics { get; set; }
	public string CareNeeds { get; set; }
	public DateTime ModifiedDate { get; set; }

}