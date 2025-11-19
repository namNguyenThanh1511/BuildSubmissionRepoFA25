using DAL.Model;
using System.Net;
using System.Text.Json;

namespace PRN231_SU25_SE170077.api
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
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = 500;
            string errorCode = "HB50001";
            string message = "Internal server error";

            if (exception is ArgumentException || exception is FormatException)
            {
                statusCode = 400;
                errorCode = "HB40001";
                message = "Missing or invalid input";
            }

            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                errorCode = errorCode,
                message = message
            });

            return context.Response.WriteAsync(result);
        }
    }

}
