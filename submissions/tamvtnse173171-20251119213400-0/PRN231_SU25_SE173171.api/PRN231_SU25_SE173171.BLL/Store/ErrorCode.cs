using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Store
{
    public static class ErrorCode
    {
        public const string ErrorCodeMissingInvalidInput = "HB40001";
        public const string ErrorCodeTokenMissingInvalid = "HB40101";
        public const string ErrorCodePermissionDenied = "HB40301";
        public const string ErrorCodeResourceNotFound = "HB40401";
        public const string ErrorCodeInternalServerError = "HB50001";
    }
}
