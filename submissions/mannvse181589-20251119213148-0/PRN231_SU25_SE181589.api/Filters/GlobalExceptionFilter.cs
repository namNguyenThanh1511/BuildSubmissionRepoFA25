using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Models.Responses.Common;

namespace PRN231_SU25_SESE181589.api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorResponse = context.Exception switch
            {
                ArgumentException => new ErrorResponse 
                { 
                    ErrorCode = "HB40001", 
                    Message = context.Exception.Message 
                },
                UnauthorizedAccessException => new ErrorResponse 
                { 
                    ErrorCode = "HB40301", 
                    Message = "Permission denied" 
                },
                KeyNotFoundException => new ErrorResponse 
                { 
                    ErrorCode = "HB40401", 
                    Message = context.Exception.Message 
                },
                _ => new ErrorResponse 
                { 
                    ErrorCode = "HB50001", 
                    Message = "Internal server error" 
                }
            };

            var statusCode = context.Exception switch
            {
                ArgumentException => 400,
                UnauthorizedAccessException => 403,
                KeyNotFoundException => 404,
                _ => 500
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
            
            context.ExceptionHandled = true;
        }
    }
}