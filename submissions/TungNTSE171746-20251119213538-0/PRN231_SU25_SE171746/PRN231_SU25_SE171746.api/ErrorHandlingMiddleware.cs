namespace PRN231_SU25_SE171746.api
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
                            await WriteJsonError(context, "HB40101", "Token missing or invalid", 401);
                            break;
                        case 403:
                            await WriteJsonError(context, "HB40301", "Permission denied", 403);
                            break;
                        case 404:
                            await WriteJsonError(context, "HB40401", "Resource not found", 404);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                if (!context.Response.HasStarted)
                {
                    int statusCode = context.Response.StatusCode;

                    if (statusCode == 401)
                    {
                        await WriteJsonError(context, "HB40101", "Token missing or invalid", 401);
                    }
                    else if (statusCode == 403)
                    {
                        await WriteJsonError(context, "HB40301", "Permission denied", 403);
                    }
                    else
                    {
                        await WriteJsonError(context, "HB50001", "Internal server error", 500);
                    }
                }
            }
        }

        private async Task WriteJsonError(HttpContext context, string errorCode, string message, int statusCode)
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
