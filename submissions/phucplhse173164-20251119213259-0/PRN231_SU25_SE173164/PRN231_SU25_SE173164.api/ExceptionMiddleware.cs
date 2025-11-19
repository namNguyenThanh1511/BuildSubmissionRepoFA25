using PRN231_SU25_SE173164.bll.Core;
using System.Text.Json;

namespace PRN231_SU25_SE173164.api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                context.Response.StatusCode = ex.ErrorCode;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    errorCode = ErrorHelper.GetErrorCode(ex.ErrorCode),
                    message = ex.ErrorMessage ?? ErrorHelper.GetErrorMessage(ex.ErrorCode)
                };

                var json = JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(json);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    errorCode = ErrorHelper.GetErrorCode(500),
                    message = ErrorHelper.GetErrorMessage(500)
                };

                var json = JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
