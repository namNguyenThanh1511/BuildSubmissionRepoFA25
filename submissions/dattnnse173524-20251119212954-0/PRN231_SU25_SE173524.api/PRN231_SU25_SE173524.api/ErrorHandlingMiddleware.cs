using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PRN231_SU25_SE173524.api
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = new ErrorResponse();
            int statusCode;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    error.ErrorCode = "HB40101";
                    error.Message = "Token missing or invalid";
                    break;

                case ValidationException vex:
                    statusCode = StatusCodes.Status400BadRequest;
                    error.ErrorCode = "HB40001";
                    error.Message = vex.Message;

                    //error.Message = "Missing or invalid input";

                    break;

                case ArgumentException aex:
                    statusCode = StatusCodes.Status400BadRequest;
                    error.ErrorCode = "HB40002";
                    error.Message = aex.Message;

                    //error.Message = "Missing or invalid input";
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    error.ErrorCode = "HB40401";
                    error.Message = exception.Message;


                    error.Message = "Resource not found";

                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    error.ErrorCode = "HB50001";
                    error.Message = "Internal server error";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(error);
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}

