using PRN231_SU25_SE173175.Repository.Base;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try { await _next(context); }
        catch (BaseException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            string errorCode = ex.StatusCode switch
            {
                400 => "HB40001",
                401 => "HB40101",
                403 => "HB40301",
                404 => "HB40401",
                500 => "HB50001",
                _ => "HB50001"
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                errorCode,
                message = ex.ErrorMessage?.ToString() ?? "Error"
            }));
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                errorCode = "HB40101",
                message = ex.Message
            }));
        }
        catch (System.Security.SecurityException ex)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                errorCode = "HB40301",
                message = ex.Message
            }));
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                errorCode = "HB40401",
                message = ex.Message
            }));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                errorCode = "HB50001",
                message = ex.Message
            }));
        }
    }
} 