namespace PRN232_SU25_SE184727.Models
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static ErrorModel Invalid()
        {
            return new ErrorModel
            {
                ErrorCode = "HB40001",
                Message = "Missing/Invalid input"
            };
        }
        public static ErrorModel Unauthor()
        {
            return new ErrorModel
            {
                ErrorCode = "HB40101",
                Message = "Token missing/invalid"
            };
        }
        public static ErrorModel PermissionDenied()
        {
            return new ErrorModel
            {
                ErrorCode = "HB40301",
                Message = "Permission denied"
            };
        }
        public static ErrorModel NotFound()
        {
            return new ErrorModel
            {
                ErrorCode = "HB40401",
                Message = "Resource not found"
            };
        }
        public static ErrorModel ServerError()
        {
            return new ErrorModel
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            };
        }
    }
}
