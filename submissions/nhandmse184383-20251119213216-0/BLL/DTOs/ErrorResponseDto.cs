using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ErrorResponseDto
    {
        public string ErrorCode { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
