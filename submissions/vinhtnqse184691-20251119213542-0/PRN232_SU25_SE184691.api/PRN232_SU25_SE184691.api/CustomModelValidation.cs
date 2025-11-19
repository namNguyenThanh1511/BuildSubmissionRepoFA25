using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace PRN232_SU25_SE184691.api;

public static class ValidationHelper
{
    public static Dictionary<string, string[]> FormatModelErrors(ModelStateDictionary modelState)
    {
        return modelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
    }
}
