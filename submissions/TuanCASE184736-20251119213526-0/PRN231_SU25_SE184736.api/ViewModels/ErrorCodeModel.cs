namespace PRN231_SU25_SE184736.api.ViewModels
{
    public class ErrorCodeModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static ErrorCodeModel InvalidInput()
        {
            return new ErrorCodeModel()
            {
                ErrorCode = "HB40001",
                Message = "Missing/invalid input"
            };
        }

        public static ErrorCodeModel Unauthorized()
        {
            return new ErrorCodeModel()
            {
                ErrorCode = "HB40101",
                Message = "Token missing/invalid"
            };
        }

        public static ErrorCodeModel Forbidden()
        {
            return new ErrorCodeModel()
            {
                ErrorCode = "HB40301",
                Message = "Permission denied"
            };
        }

        public static ErrorCodeModel NotFound()
        {
            return new ErrorCodeModel()
            {
                ErrorCode = "HB40401",
                Message = "Resource not found"
            };
        }

        public static ErrorCodeModel ServerError()
        {
            return new ErrorCodeModel()
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            };
        }

    }
}
