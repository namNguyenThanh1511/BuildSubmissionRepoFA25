using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Responses
{
    public class ApiException : Exception
    {
        public string ErrorCode { get; }

        public ApiException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
