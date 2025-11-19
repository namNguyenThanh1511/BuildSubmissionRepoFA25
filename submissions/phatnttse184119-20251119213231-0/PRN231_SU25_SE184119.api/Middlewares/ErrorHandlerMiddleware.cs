using System.Net;
using System.Text.Json;

namespace PRN231_SU25_SE184119.api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Kiểm tra response code sau khi xử lý xong
                if (!context.Response.HasStarted)
                {
                    switch (context.Response.StatusCode)
                    {
                        case 401:
                            await WriteJsonErrorAsync(context, "HB40101", "Token missing/invalid");
                            break;
                        case 403:
                            await WriteJsonErrorAsync(context, "HB40301", "Permission denied");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                errorCode = "HB50001",
                message = "Internal server error"
            };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Xử lý cụ thể từng loại exception nếu cần
            if (exception is UnauthorizedAccessException)
            {
                response.errorCode = "HB40101";
                response.message = "Unauthorized";
                context.Response.StatusCode = 401;
            }
            else if (exception is ArgumentException)
            {
                response.errorCode = "HB40001";
                response.message = exception.Message;
                context.Response.StatusCode = 400;
            }
            else if (exception is KeyNotFoundException)
            {
                response.errorCode = "HB40401";
                response.message = exception.Message;
                context.Response.StatusCode = 404;
            }

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }

        private class ErrorResponse
        {
            public string errorCode { get; set; }
            public string message { get; set; }
        }

        private async Task WriteJsonErrorAsync(HttpContext context, string errorCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorCode switch
            {
                "HB40101" => 401,
                "HB40301" => 403,
                _ => 500
            };

            var error = new ErrorResponse
            {
                errorCode = errorCode,
                message = message
            };

            var result = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(result);
        }
    }
}
