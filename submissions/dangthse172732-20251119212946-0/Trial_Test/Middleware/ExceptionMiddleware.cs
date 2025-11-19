using BLL.Exception;

namespace Trial_Test.api.Middleware
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

                // Nếu là ModelState không hợp lệ (400)
                if (context.Response.StatusCode == 400 && context.Items.ContainsKey("ModelStateError"))
                {
                    await WriteError(context, 400, "HB40001", context.Items["ModelStateError"]?.ToString() ?? "Missing or invalid input");
                }
                // Nếu trả về 401 không có token
                else if (context.Response.StatusCode == 401)
                {
                    await WriteError(context, 401, "HB40101", "Token missing/invalid");
                }
                // Nếu trả về 403
                else if (context.Response.StatusCode == 403)
                {
                    await WriteError(context, 403, "HB40301", "Permission denied");
                }
                // Nếu trả về 404
                else if (context.Response.StatusCode == 404)
                {
                    await WriteError(context, 404, "HB40401", "Resource not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteError(context, 500, "HB50001", "Internal server error");
            }
        }

        private async Task WriteError(HttpContext context, int statusCode, string errorCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var error = new ApiError
            {
                ErrorCode = errorCode,
                Message = message
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }

}
