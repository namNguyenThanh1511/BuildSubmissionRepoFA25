using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;

namespace Helper
{
    public static class ErrorResultHelper
    {
        public static IActionResult Create(HttpStatusCode statusCode)
        {
            var (errorCode, message) = ErrorMap.GetError(statusCode);
            var result = new
            {
                errorCode,
                message
            };

            return statusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(result),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(result),
                HttpStatusCode.Forbidden => new ObjectResult(result) { StatusCode = 403 },
                HttpStatusCode.NotFound => new NotFoundObjectResult(result),
                _ => new ObjectResult(result) { StatusCode = 500 },
            };
        }
    }
}
