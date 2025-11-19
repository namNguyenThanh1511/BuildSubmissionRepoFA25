using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170489.DAL.ModelExtensions;

namespace PRN231_SU25_SE170489.API.Extensions
{
	public static class ResultExtensions
	{
		public static IActionResult ToActionResult<T>(this Result<T> result)
		{
			if (result.Success)
				return new OkObjectResult(result.Data);

			var statusCode = result.Error.ErrorCode switch
			{
				"HB40001" => 400,
				"HB40101" => 401,
				"HB40301" => 403,
				"HB40401" => 404,
				"HB50001" => 500,
				_ => 500
			};

			return new ObjectResult(result.Error) { StatusCode = statusCode };
		}
		public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
		{
			return result.ToActionResult();
		}
	}
}
