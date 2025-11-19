using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ErrorResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static ErrorResult InvalidInput(string errorCode = "HB40001", string message = "Missing/invalid input")
        {
            return new ErrorResult
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static ErrorResult TokenMissing(string errorCode = "HB40101", string message = "Token invalid")
        {
            return new ErrorResult
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static ErrorResult PermissionDenied(string errorCode = "HB40301", string message = "Permission denied")
        {
            return new ErrorResult
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static ErrorResult ResourceNotFound(string errorCode = "HB40401", string message = "Resource not found")
        {
            return new ErrorResult
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static ErrorResult ServerError(string errorCode = "HB50001", string message = "Internal server error")
        {
            return new ErrorResult
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

    }
}
