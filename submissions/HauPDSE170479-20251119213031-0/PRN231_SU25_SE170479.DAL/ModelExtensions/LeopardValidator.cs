using PRN231_SU25_SE170479.DAL.DTOs;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE170479.DAL.ModelExtensions
{
	public static class LeopardValidator
	{
		private static readonly Regex ModelNameRegex = new("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

		public static Result<string> Validate(LeopardDTO dto)
		{
			if (!ModelNameRegex.IsMatch(dto.LeopardName))
				return Errors.ValidationError<string>("LeopardName is required");

			if (dto.Weight > 15)
				return Errors.ValidationError<string>("Weight must be greater than 0");

			return Result<string>.Ok();
		}
	}
}