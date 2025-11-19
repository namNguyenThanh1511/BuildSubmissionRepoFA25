using System.Text.Json;

namespace PRN231_SU25_SE181526.api
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == 403)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        errorCode = "HB40301",
                        message = "Permission denied"
                    }));
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                var errorCode = context.Response.StatusCode switch
                {
                    400 => "HB40001",
                    401 => "HB40101",
                    404 => "HB40401",
                    500 => "HB50001",
                    _ => "HB50001"
                };

                var result = JsonSerializer.Serialize(new
                {
                    errorCode,
                    message = ex.Message
                });

                await context.Response.WriteAsync(result);
            }
        }
    }

}
