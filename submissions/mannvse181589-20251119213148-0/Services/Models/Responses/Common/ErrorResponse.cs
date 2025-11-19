namespace Services.Models.Responses.Common
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}