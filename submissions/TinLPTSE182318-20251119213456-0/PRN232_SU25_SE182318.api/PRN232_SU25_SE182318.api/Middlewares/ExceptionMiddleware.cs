using BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace PRN232_SU25_SE182318.api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)ex.StatusCode;

                var errorResponse = new
                {
                    errorCode = ex.ErrorCode,
                    message = ex.Message
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await httpContext.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    errorCode = "HB50001",
                    message = "Internal server error"
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await httpContext.Response.WriteAsync(json);
            }

            if (!httpContext.Response.HasStarted &&
                httpContext.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                httpContext.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(new
                {
                    errorCode = "HB40001",
                    message = "Missing/Invalid input"
                });

                await httpContext.Response.WriteAsync(json);
            }

            if (!httpContext.Response.HasStarted)
            {
                switch (httpContext.Response.StatusCode)
                {
                    case 401:
                        await WriteErrorAsync(httpContext, "HB40101", "Token missing/Invalid");
                        break;
                    case 403:
                        await WriteErrorAsync(httpContext, "Hb40301", "Permission denied");
                        break;
                    case 404:
                        await WriteErrorAsync(httpContext, "Hb40401", "Resource not found");
                        break;
                }
            }
        }

        private async Task WriteErrorAsync(HttpContext context, string errorCode, string message)
        {
            context.Response.ContentType = "application/json";
            var response = new { errorCode, message };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

}
