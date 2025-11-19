using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170197.api.Helper;

namespace PRN231_SU25_SE170197.api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly ILeopardService _leopardService;
        public LeopardController(ILeopardService leopardService)
        {
            _leopardService = leopardService;
        }

        [Authorize(Roles = "4,5,6,7")] // administrator, moderator, developer, member
        [HttpGet("LeopardProfile")]
        public async Task<IActionResult> GetAllHandbags()
        {
            try
            {
                var handbags = await _leopardService.GetAllLeopard();
                return new OkObjectResult(handbags);
            }
            catch
            {
                return ErrorResponseHelper.FromCode("HB50001"); //Internal server error
            }
        }

        [Authorize(Roles = "4,5,6,7")] 
        [HttpGet("LeopardProfile/{id}")]
        public async Task<IActionResult> GetHandbagById(int id)
        {
            try
            {
                var handbag = await _leopardService.GetLeopardById(id);
                if (handbag == null)
                {
                    return ErrorResponseHelper.FromCode("HB40401");  //Resource not found
                }
                return new OkObjectResult(handbag);
            }
            catch
            {
                return ErrorResponseHelper.FromCode("HB50001"); //Internal server error
            }
        }

        [Authorize(Roles = "5,6")] // administrator, moderator
        [HttpDelete("LeopardProfile/{id}")]
        public async Task<IActionResult> DeleteLeopard(int id)
        {
            try
            {
                await _leopardService.DeleteLeopard(id);
                return new OkObjectResult("Delete successfully");
            }
            catch (ArgumentException ex)
            {
                return ErrorResponseHelper.FromCode("HB40401"); //Resource not found
            }
            catch
            {
                return ErrorResponseHelper.FromCode("HB50001"); //Internal server error
            }
        }
    }
}
