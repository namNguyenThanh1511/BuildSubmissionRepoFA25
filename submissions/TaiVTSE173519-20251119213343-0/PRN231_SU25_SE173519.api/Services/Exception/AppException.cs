using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exception
{
    public class AppException : System.Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public AppException(string errorCode, string message, int statusCode = 400) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
