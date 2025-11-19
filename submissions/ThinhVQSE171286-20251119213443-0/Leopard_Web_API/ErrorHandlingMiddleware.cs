namespace Leopard_Web_API
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

                if (!context.Response.HasStarted)
                {
                    switch (context.Response.StatusCode)
                    {
                        case 401:
                            await WriteJsonError(context, "HB40101", "Token missing or invalid");
                            break;
                        case 403:
                            await WriteJsonError(context, "HB40301", "Permission denied");
                            break;
                        case 404:
                            await WriteJsonError(context, "HB40401", "Resource not found");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                if (!context.Response.HasStarted)
                {
                    await WriteJsonError(context, "HB50001", "Internal server error", 500);
                }
            }
        }

        private async Task WriteJsonError(HttpContext context, string errorCode, string message, int statusCode = 500)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                errorCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}