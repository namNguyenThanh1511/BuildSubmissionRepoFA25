namespace PRN231_SU25_SE184437.DAL.ModelExtensions
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public DetailError? DetailError { get; set; }
    }

    public class DetailError
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; }
    }
}
