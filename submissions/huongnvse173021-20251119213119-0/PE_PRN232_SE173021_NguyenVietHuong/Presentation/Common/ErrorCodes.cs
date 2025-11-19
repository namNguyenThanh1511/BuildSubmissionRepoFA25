using Microsoft.AspNetCore.Mvc;


namespace Presentation.Common
{
    public static class ErrorCodes
    {
        public const string MissingOrInvalidInput = "HB40001";
        public const string TokenMissingOrInvalid = "HB40101";
        public const string PermissionDenied = "HB40301";
        public const string ResourceNotFound = "HB40401";
        public const string InternalServerError = "HB50001";
    }
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
    public static class ErrorHelper
    {
        public static IActionResult BadRequest(string message) =>
            new BadRequestObjectResult(new ErrorResponse(ErrorCodes.MissingOrInvalidInput, message));

        public static IActionResult Unauthorized(string message) =>
            new UnauthorizedObjectResult(new ErrorResponse(ErrorCodes.TokenMissingOrInvalid, message));

        public static IActionResult Forbidden(string message) =>
            new ObjectResult(new ErrorResponse(ErrorCodes.PermissionDenied, message))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

        public static IActionResult NotFound(string message) =>
            new NotFoundObjectResult(new ErrorResponse(ErrorCodes.ResourceNotFound, message));

        public static IActionResult InternalServerError(string message) =>
            new ObjectResult(new ErrorResponse(ErrorCodes.InternalServerError, message))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
    }
}
