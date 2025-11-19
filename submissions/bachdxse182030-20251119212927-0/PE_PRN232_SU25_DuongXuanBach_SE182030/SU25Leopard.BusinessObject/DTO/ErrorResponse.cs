using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SU25Leopard.BusinessObject.DTO
{
    public static class ErrorResponse
    {
        public static object BadRequest(string message) => new { errorCode = "HB40001", message };
        public static object Unauthorized(string message = "Token missing/invalid") => new { errorCode = "HB40101", message };
        public static object Forbidden(string message = "Permission denied") => new { errorCode = "HB40301", message };
        public static object NotFound(string message = "Resource not found") => new { errorCode = "HB40401", message };
        public static object InternalError(string message = "Internal server error") => new { errorCode = "HB50001", message };
    }
}
