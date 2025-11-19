using PRN231_SU25_SE170489.DAL.DTOs;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE170489.DAL.ModelExtensions
{
	public static class LeopardProfileValidator
	{
		private static readonly Regex LeopardNameRegex = new("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

		public static Result<string> Validate(LeopardProfileDTO dto)
		{
			if (!LeopardNameRegex.IsMatch(dto.LeopardName))
				return Errors.ValidationError<string>("leopardName is required");

			if (dto.Weight < 15)
				return Errors.ValidationError<string>("weight must be greater than 15");

			if (dto.Characteristics == null)
				return Errors.ValidationError<string>("Characteristics is required");

			if (dto.CareNeeds == null)
				return Errors.ValidationError<string>("CareNeeds is required");

			if (dto.ModifiedDate == null)
				return Errors.ValidationError<string>("ModifiedDate is required");

			if (dto.LeopardTypeId == null)
				return Errors.ValidationError<string>("LeopardTypeId is required");

			return Result<string>.Ok();
		}
	}
}