using SU25Leopard.BusinessObject.DTO;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Handle 401 and 403 responses
        if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            var response = ErrorResponse.Unauthorized();
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        else if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            var response = ErrorResponse.Forbidden();
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}