using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models.Responses.Common;

namespace PRN231_SU25_SESE181589.api.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult BadRequestWithModelErrors(this ControllerBase controller)
        {
            var errors = string.Join("; ", controller.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            
            return controller.BadRequest(new ErrorResponse 
            { 
                ErrorCode = "HB40001", 
                Message = errors 
            });
        }
    }
}