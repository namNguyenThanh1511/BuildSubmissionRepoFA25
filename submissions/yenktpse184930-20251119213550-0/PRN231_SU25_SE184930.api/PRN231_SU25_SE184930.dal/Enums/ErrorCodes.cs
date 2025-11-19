using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Enums
{
    public static class ErrorCodes
    {
        public const string BadRequest = "HB40001";
        public const string Unauthorized = "HB40101";
        public const string Forbidden = "HB40301";
        public const string NotFound = "HB40401";
        public const string InternalServerError = "HB50001";
    }
}
