using System.Net;

namespace DAL.Model
{
    public static class ErrorMap
    {
        private static readonly Dictionary<HttpStatusCode, (string ErrorCode, string Message)> ErrorMappings = new()
        {
            { HttpStatusCode.BadRequest, ("HB40001", "Missing/invalid input") },
            { HttpStatusCode.Unauthorized, ("HB40101", "Token missing/invalid") },
            { HttpStatusCode.Forbidden, ("HB40301", "Permission denied") },
            { HttpStatusCode.NotFound, ("HB40401", "Resource not found") },
            { HttpStatusCode.InternalServerError, ("HB50001", "Internal server error") },
        };

        public static (string errorCode, string message) GetError(HttpStatusCode statusCode)
        {
            return ErrorMappings.TryGetValue(statusCode, out var value)
                ? value
                : ("HB50001", "Internal server error");
        }
    }

}
