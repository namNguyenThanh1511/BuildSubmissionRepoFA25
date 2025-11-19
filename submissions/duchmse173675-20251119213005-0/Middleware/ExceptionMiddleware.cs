using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

namespace PRN231_SU25_SE173675.api.Middleware
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response has already started, the exception middleware will not run.");
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorCode = "HB50001";
            var message = "Internal server error";

            switch (ex)
            {
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "HB40001";
                    message = ex.Message;
                    break;

                case ApplicationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "HB40001";
                    message = ex.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    errorCode = "HB40301";
                    message = "Invalid email or password / Permission denied";
                    break;

                case SecurityTokenExpiredException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorCode = "HB40401";
                    message = "Token expired";
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = "HB40401";
                    message = ex.Message;
                    break;

                case JsonException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "HB40001";
                    message = "Malformed JSON request";
                    break;
            }

            _logger.LogError(ex, "Unhandled exception occurred");

            var result = JsonConvert.SerializeObject(new
            {
                errorCode,
                message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
