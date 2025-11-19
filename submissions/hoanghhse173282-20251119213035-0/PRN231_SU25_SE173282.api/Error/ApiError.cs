using System.Text.Json.Serialization;

namespace PRN231_SU25_SE173282.api.Error
{
    public class ApiError
    {
        [JsonPropertyName("ErrorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<FieldError>? Errors { get; set; }

        public ApiError(string errorCode, string message, List<FieldError>? errors = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Errors = errors;
        }
    }

    public class FieldError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
