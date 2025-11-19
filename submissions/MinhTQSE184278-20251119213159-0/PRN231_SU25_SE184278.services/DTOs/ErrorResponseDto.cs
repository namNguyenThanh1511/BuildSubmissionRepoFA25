using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.services.DTOs
{
    public class ErrorResponseDto
    {
        public string ErrorCode { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
