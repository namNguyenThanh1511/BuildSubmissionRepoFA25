using api.dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.middleware
{
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
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context, ErrorCodeEnum.HB50001, 500);
                return;
            }

            if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
            {
                await HandleExceptionAsync(context, ErrorCodeEnum.HB40101, 401);
            }
            else if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
            {
                await HandleExceptionAsync(context, ErrorCodeEnum.HB40301, 403);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, ErrorCodeEnum code, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var error = ErrorResponse.FromCode(code);
            var json = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        }
    }
}
