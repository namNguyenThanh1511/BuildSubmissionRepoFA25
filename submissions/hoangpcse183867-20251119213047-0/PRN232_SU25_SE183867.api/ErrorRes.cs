namespace PRN232_SU25_SE183867.api
{
    public class ErrorRes
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorRes(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public static ErrorRes NotFound(string message)
        {
            return new ErrorRes("HB40401", message);
        }

        public static ErrorRes InternalErrorServer(string message)
        {
            return new ErrorRes("HB50001", message);
        }

        public static ErrorRes Unauthorized(string message)
        {
            return new ErrorRes("HB40101", message);
        }

        public static ErrorRes Forbidden(string message)
        {
            return new ErrorRes("HB40301", message);
        }

        public static ErrorRes BadRequest (string message)
        {
            return new ErrorRes("HB4001", message);
        }
    }
}
