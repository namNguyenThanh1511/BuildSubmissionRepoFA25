using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class CustomException : Exception
    {
        public string ErrorCode { get; }
        public HttpStatusCode StatusCode { get; }

        public CustomException(string errorCode, string message, HttpStatusCode statusCode) : base(message) 
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
