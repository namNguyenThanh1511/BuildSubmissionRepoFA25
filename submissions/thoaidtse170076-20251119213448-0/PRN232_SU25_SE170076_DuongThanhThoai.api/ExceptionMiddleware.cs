using System.Security;
using System.Text.Json;

namespace PRN232_SU25_SE170076.api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred at {Time}", DateTime.Now);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = 500;
            string errorCode = "HB50001";
            string message = "Internal server error";

            // Xử lý các trường hợp cụ thể
            if (exception is UnauthorizedAccessException)
            {
                statusCode = 401;
                errorCode = "HB40101";
                message = "Token missing/invalid";
            }
            else if (exception is ArgumentException || exception is FormatException)
            {
                statusCode = 400;
                errorCode = "HB40001";
                message = "Missing/invalid input";
            }
            else if (exception is SecurityException)
            {
                statusCode = 403;
                errorCode = "HB40301";
                message = "Permission denied";
            }

            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                errorCode = errorCode,
                message = message,
                timestamp = DateTime.Now 
            });

            return context.Response.WriteAsync(result);
        }
    }
}