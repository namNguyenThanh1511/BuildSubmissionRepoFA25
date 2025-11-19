using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE182328.api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Error(string errorCode, string message, int statusCode)
        {
            return StatusCode(statusCode, new { errorCode, message });
        }
    }
}