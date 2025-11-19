namespace PRN231_SU25_SE173635.api.Models.DTOs;

public class ErrorResponse
{
    public string ErrorCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public static class ErrorCodes
{
    public const string MissingInput = "HB40001";
    public const string TokenMissing = "HB40101";
    public const string PermissionDenied = "HB40301";
    public const string ResourceNotFound = "HB40401";
    public const string InternalServerError = "HB50001";
} 