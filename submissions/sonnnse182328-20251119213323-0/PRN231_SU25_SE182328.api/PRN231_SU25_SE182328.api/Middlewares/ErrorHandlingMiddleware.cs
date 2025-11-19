using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            // Handle 401, 403 after pipeline
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { errorCode = "HB40101", message = "Token missing/invalid" });
                await context.Response.WriteAsync(result);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { errorCode = "HB40301", message = "Permission denied" });
                await context.Response.WriteAsync(result);
            }
        }
        catch
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { errorCode = "HB50001", message = "Internal server error" });
            await context.Response.WriteAsync(result);
        }
    }
}