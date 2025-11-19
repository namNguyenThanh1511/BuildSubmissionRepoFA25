using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Repository.Response;
using System.Text.Json;

namespace PRN232_SU25_SE170587.api
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse("HB40301", "Permission denied");

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            if (authorizeResult.Challenged)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse("HB40101", "Token missing/invalid");                

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            // Nếu không lỗi thì tiếp tục bình thường
            await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

}
