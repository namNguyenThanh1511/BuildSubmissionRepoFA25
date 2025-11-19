namespace PRN231_SU25_SE173566.api
{
	using Microsoft.AspNetCore.Http;
	using System;
	using System.Net;
	using System.Text.Json;
	using System.Threading.Tasks;

	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError;
			var errorCode = ErrorCodes.InternalServerError;
			var message = "Internal server error";

			if (exception is UnauthorizedAccessException)
			{
				code = HttpStatusCode.Unauthorized;
				errorCode = ErrorCodes.Unauthorized;
				message = "Token missing/invalid";
			}

			var result = JsonSerializer.Serialize(new ErrorResponse
			{
				ErrorCode = errorCode,
				Message = message
			});

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
	}
}
