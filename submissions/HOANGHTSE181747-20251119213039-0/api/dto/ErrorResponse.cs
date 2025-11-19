namespace api.dto
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static ErrorResponse FromCode(ErrorCodeEnum code)
        {
            return code switch
            {
                ErrorCodeEnum.HB40001 => new ErrorResponse { ErrorCode = "HB40001", Message = "Missing or invalid input" },
                ErrorCodeEnum.HB40101 => new ErrorResponse { ErrorCode = "HB40101", Message = "Token missing or invalid" },
                ErrorCodeEnum.HB40301 => new ErrorResponse { ErrorCode = "HB40301", Message = "Permission denied" },
                ErrorCodeEnum.HB40401 => new ErrorResponse { ErrorCode = "HB40401", Message = "Resource not found" },
                ErrorCodeEnum.HB50001 => new ErrorResponse { ErrorCode = "HB50001", Message = "Internal server error" },
                _ => new ErrorResponse { ErrorCode = "HB50001", Message = "Unexpected error" }
            };
        }

    }
    public enum ErrorCodeEnum
    {
        HB40001,
        HB40101, 
        HB40301, 
        HB40401, 
        HB50001  
    }

}
