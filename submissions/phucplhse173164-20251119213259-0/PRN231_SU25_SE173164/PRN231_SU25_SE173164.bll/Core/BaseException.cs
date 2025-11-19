using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173164.bll.Core
{
    public class BaseException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public BaseException(int errorCode, string errorMessage) : base(errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
