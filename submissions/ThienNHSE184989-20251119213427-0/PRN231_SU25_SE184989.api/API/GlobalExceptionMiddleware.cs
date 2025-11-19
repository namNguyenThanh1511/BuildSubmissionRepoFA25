using System.Text.Json;

namespace API
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Only handle 404 status codes (401/403 handled by JWT events)
                if (!context.Response.HasStarted && context.Response.StatusCode == 404)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { errorCode = "HB40401", message = "Resource not found" }));
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, errorCode, message) = ex switch
            {
                ArgumentException => (400, "HB40001", ex.Message),
                UnauthorizedAccessException => (401, "HB40101", ex.Message),
                InvalidOperationException => (403, "HB40301", ex.Message),
                KeyNotFoundException => (404, "HB40401", ex.Message),
                _ => (500, "HB50001", "Internal server error")
            };

            
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new { errorCode, message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
