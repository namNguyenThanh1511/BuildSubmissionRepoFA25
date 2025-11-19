using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173164.bll.Core
{
    public static class ErrorHelper
    {
        private const string ErrorCodePrefix = "HB";
        private const string ErrorCodeSuffix = "01";

        public static Dictionary<int, string> ErrorList = new Dictionary<int, string>
        {
            { 400, "Missing/invalid input" },
            { 401, "Token missing/invalid" },
            { 403, "Permission denied" },
            { 404, "Resource not found" },
            { 500, "Internal server error" }
        };

        public static string GetErrorMessage(int errorCode)
        {
            if (ErrorList.TryGetValue(errorCode, out var message))
            {
                return message;
            }
            return "Unknown error occurred";
        }
        public static string GetErrorCode(int errorCode)
        {
            return $"{ErrorCodePrefix}{errorCode:D3}{ErrorCodeSuffix}";
        }
    }
}
