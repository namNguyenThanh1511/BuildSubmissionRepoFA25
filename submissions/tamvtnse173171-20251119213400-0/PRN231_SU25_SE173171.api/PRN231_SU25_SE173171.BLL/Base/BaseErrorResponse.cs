using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Base
{
    public class BaseErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public BaseErrorResponse(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
