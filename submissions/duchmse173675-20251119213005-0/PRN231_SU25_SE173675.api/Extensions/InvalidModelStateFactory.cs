using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173675.api.Extensions
{
    public static class InvalidModelStateFactory
    {
        public static IActionResult GenerateCustomValidationResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(ms => ms.Value.Errors.Select(err =>
                    err.ErrorMessage
                ))
                .ToList();

            return new BadRequestObjectResult(new
            {
                errorCode = "HB40001",
                messages = errors
            });
        }
    }
}
