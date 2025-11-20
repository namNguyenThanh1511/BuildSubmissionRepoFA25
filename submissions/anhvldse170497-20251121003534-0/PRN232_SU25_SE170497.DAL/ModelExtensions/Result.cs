namespace PRN232_SU25_SE170497.DAL.ModelExtensions
{
    public record ErrorResponse(string ErrorCode, string Message);

    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public ErrorResponse Error { get; set; }

        public static Result<T> Ok(T data = default) => new() { Success = true, Data = data };
        public static Result<T> Fail(string code, string message) => new() { Success = false, Error = new(code, message) };
    }
}