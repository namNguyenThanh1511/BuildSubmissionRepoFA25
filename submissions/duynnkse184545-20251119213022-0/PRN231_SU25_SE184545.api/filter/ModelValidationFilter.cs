using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PRN231_SU25_SE184545.api.filter
{
    public class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                var errorMessage = errors.FirstOrDefault() ?? "Invalid input";

                context.Result = new BadRequestObjectResult(new
                {
                    errorCode = "HB40001",
                    message = errorMessage
                });
            }
        }
    }

}
