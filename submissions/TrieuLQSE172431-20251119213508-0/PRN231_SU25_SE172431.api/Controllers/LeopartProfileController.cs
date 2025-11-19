using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE172431.BLL.Service;

namespace PRN231_SU25_SE172431.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopartProfileController : ControllerBase
    {
     
        private readonly ILeopardProfileService _service;
        public LeopartProfileController(ILeopardProfileService service)
        {
            _service = service;
        }
        [EnableQuery]
        [HttpGet()]
        [Authorize(Roles = "  1 , 6 , 7, 4")]

        public async Task<IActionResult> Search( )
        {
            var result = await _service.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "  1 , 6 , 7 , 4")]

        public async Task<IActionResult> GetById(int id)
        {
            var handbag = await _service.GetById(id);
            return Ok(handbag);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "1, 6")]

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteLeopardProfileAsync(id);
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                message = "LeoPardProfile deleted successfully."
            });
        }

    }
}
