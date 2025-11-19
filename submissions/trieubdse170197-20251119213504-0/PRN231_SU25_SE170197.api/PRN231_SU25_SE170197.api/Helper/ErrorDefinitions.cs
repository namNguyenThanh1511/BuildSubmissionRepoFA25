using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE170197.api.Helper
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
    }
}
