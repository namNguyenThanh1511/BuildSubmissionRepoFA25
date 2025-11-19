using System.Net;
using PRN231_SU25_SE184386.DAL.ModelExtensions;

namespace PRN231_SU25_SE184386.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 401 ||
                    httpContext.Response.StatusCode == 403)
                {
                    httpContext.Response.ContentType = "application/json";

                    var response = new ApiResponse<string> { Success = false };
                    var detailError = new DetailError();

                    if (httpContext.Response.StatusCode == ((int)HttpStatusCode.Unauthorized))
                    {
                        detailError.ErrorCode = "HB40101";
                        detailError.Message = "Token missing/invalid";
                    }

                    if (httpContext.Response.StatusCode == ((int)(HttpStatusCode.Forbidden)))
                    {
                        detailError.ErrorCode = "HB40301";
                        detailError.Message = "Permission denied";
                    }

                    response.DetailError = detailError;

                    await httpContext.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception)
            {
                var errorResponse = new ApiResponse<string> { Success = false };
                var detailError = new DetailError
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                };

                errorResponse.DetailError = detailError;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
