using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Responses
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
