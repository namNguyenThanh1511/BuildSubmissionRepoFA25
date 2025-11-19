using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE182972.ErrorModel
{
    public static class ErrorResponse
    {
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
