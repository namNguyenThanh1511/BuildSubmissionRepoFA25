using Services.Exception;
using System.Text.Json;

namespace PRN231_SU25_SE173519.api.Middleware
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

                if (context.Response.StatusCode == 400 && context.Items.ContainsKey("ModelStateError"))
                {
                    await WriteError(context, 400, "HB40001", context.Items["ModelStateError"]?.ToString() ?? "Missing/invalid input");
                }
                else if (context.Response.StatusCode == 401)
                {
                    await WriteError(context, 401, "HB40101", "Token missing/invalid");
                }
                else if (context.Response.StatusCode == 403)
                {
                    await WriteError(context, 403, "HB40301", "Permission denied");
                }
                else if (context.Response.StatusCode == 404)
                {
                    await WriteError(context, 404, "HB40401", "Resource not found");
                }
            }
            catch (AppException ex)
            {
                await WriteError(context, ex.StatusCode, ex.ErrorCode, ex.Message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteError(context, 500, "HB50001", "Internal server error");
            }
        }

        private async Task WriteError(HttpContext context, int statusCode, string errorCode, string message)
        {
            if (context.Response.HasStarted) return;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var error = new { errorCode, message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}
