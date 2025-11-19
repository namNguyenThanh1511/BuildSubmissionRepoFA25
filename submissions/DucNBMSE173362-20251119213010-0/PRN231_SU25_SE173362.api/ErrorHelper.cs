using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PRN231_SU25_SE173362.api
{
    public static class ErrorHelper
    {
        public static IActionResult UnauthorizedResult()
        => new ObjectResult(new ApiError("HB40101", "Token missing/invalid")) { StatusCode = 401 };

        public static IActionResult ForbiddenResult()
            => new ObjectResult(new ApiError("HB40301", "Permission denied")) { StatusCode = 403 };

        public static IActionResult NotFoundResult(string message = "Resource not found")
            => new ObjectResult(new ApiError("HB40401", message)) { StatusCode = 404 };

        public static IActionResult BadRequestResult(string message = "Missing/invalid input")
            => new ObjectResult(new ApiError("HB40001", message)) { StatusCode = 400 };

        public static IActionResult BadRequestResult(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new FieldError
                {
                    Field = e.Key,
                    Message = e.Value.Errors.First().ErrorMessage
                })
                .ToList();

            var error = new ApiError("HB40001", "Validation failed", errors);
            return new ObjectResult(error) { StatusCode = 400 };
        }

        public static IActionResult InternalServerErrorResult(string message = "Internal server error")
            => new ObjectResult(new ApiError("HB50001", message)) { StatusCode = 500 };
    }
}
