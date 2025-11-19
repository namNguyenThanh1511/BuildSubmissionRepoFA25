namespace PRN231_SU25_SE184714.api.Models
{
    public class ErrorCodeModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static ErrorCodeModel Invalid()
        {
            return new ErrorCodeModel
            {
                ErrorCode = "HB40001",
                Message = "Missing/invalid input"
            };
        }
        public static ErrorCodeModel Unauthor()
        {
            return new ErrorCodeModel
            {
                ErrorCode = "HB40101",
                Message = "Token missing/invalid"
            };
        }
        public static ErrorCodeModel Deny()
        {
            return new ErrorCodeModel
            {
                ErrorCode = "HB40301",
                Message = "Permission denied"
            };
        }
        public static ErrorCodeModel NotFound()
        {
            return new ErrorCodeModel
            {
                ErrorCode = "HB40401",
                Message = "Resource not found"
            };
        }
        public static ErrorCodeModel InternalException()
        {
            return new ErrorCodeModel
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            };
        }
    }
}
