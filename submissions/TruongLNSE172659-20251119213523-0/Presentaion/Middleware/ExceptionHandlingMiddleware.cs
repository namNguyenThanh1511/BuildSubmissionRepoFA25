using System.Text.Json;
using System.Text;

namespace Presentaion.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            try
            {
                await _next(context);

                if (context.Response.StatusCode >= 400)
                {
                    var status = context.Response.StatusCode;

                    memStream.Seek(0, SeekOrigin.Begin);
                    var errorResult = JsonSerializer.Serialize(GetDefaultError(status));

                    context.Response.Body = originalBody;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength = Encoding.UTF8.GetByteCount(errorResult);
                    await context.Response.WriteAsync(errorResult);
                }
                else
                {
                    memStream.Seek(0, SeekOrigin.Begin);
                    context.Response.Body = originalBody;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            catch (Exception)
            {
                context.Response.Body = originalBody;
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var error = JsonSerializer.Serialize(new
                {
                    errorCode = "HB50001",
                    message = "Internal server error"
                });

                await context.Response.WriteAsync(error);
            }
        }



        private object GetDefaultError(int statusCode) =>
                        statusCode switch
                        {
                            400 => new { errorCode = "HB400", message = "Missing/invalid input" },
                            401 => new { errorCode = "HB40101", message = "Token missing/invalid" },
                            403 => new { errorCode = "HB40301", message = "Permission denied" },
                            404 => new { errorCode = "HB40401", message = "Resource not found" },
                            500 => new { errorCode = "HB50001", message = "Internal server error" },
                            _ => new { errorCode = $"HB{statusCode}", message = $"HTTP ERROR {statusCode}" }
                        };




    }
}
