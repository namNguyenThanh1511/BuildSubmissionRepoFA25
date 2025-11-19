using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public static class ErrorDefinitions
    {
        public static readonly Dictionary<string, (Func<object, IActionResult> Factory, string Message)> ErrorMap =
        new()
        {
         { "HB40001", (obj => new BadRequestObjectResult(obj), "Missing/invalid input") },
         { "HB40101", (obj => new UnauthorizedObjectResult(obj), "Token missing/invalid") },
         { "HB40301", (obj => new ObjectResult(obj) { StatusCode = 403 }, "Permission denied") },
         { "HB40401", (obj => new NotFoundObjectResult(obj), "Resource not found") },
         { "HB50001", (obj => new ObjectResult(obj) { StatusCode = 500 }, "Internal server error") },
        };

        public static IActionResult FromCode(string code)
        {
            return FromCode(code, null);
        }

        public static IActionResult FromCode(string code, string customMessage)
        {
            if (ErrorDefinitions.ErrorMap.TryGetValue(code, out var entry))
            {
                var error = new
                {
                    errorCode = code,
                    message = customMessage ?? entry.Message
                };

                return entry.Factory(error); // returns BadRequestObjectResult, etc.
            }

            // Fallback
            var unknown = new
            {
                errorCode = code,
                message = customMessage ?? "Unknown error"
            };

            return new ObjectResult(unknown) { StatusCode = 500 };
        }
    }
}
