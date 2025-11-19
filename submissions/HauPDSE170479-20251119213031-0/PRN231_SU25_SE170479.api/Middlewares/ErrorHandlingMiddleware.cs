using PRN231_SU25_SE170479.DAL.ModelExtensions;

namespace PRN231_SU25_SE170479.api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    await WriteErrorResponse(context, "HB40101", "Token missing/invalid");
                }
                else if (context.Response.StatusCode == 403)
                {
                    await WriteErrorResponse(context, "HB40301", "Permission denied");
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await WriteErrorResponse(context, "HB50001", "Internal server error");
            }
        }

        private async Task WriteErrorResponse(HttpContext context, string errorCode, string message)
        {
            context.Response.ContentType = "application/json";
            var error = new ErrorResponse(errorCode, message);
            await context.Response.WriteAsJsonAsync(error);
        }
    }

    public static class CustomAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
