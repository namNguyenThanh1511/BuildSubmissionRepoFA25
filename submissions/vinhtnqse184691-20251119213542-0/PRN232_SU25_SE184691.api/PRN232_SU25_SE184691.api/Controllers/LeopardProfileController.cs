using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repository.DTO;
using Service;

namespace PRN232_SU25_SE184691.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _serv;

        public LeopardProfileController(LeopardProfileService serv)
        {
            _serv = serv;
        }

        [Authorize]
        [EnableQuery]
        [HttpGet("search")]
        public async Task<IActionResult> GetAllOData([FromQuery]string LeopardName, double weight)
        {

            var result = await _serv.GetAllQueryable();

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });

            return StatusCode(result.code, new
            {
                Result = result.item
            });
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _serv.GetAll();

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });

            return StatusCode(result.code, new
            {
                Result = result.item
            });
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var result = await _serv.GetById(id);

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });

            return StatusCode(result.code, new
            {
                Result = result.item
            });
        }

        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeopardProfileCreate form)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    errorCode = "HB40001",
                    message = ValidationHelper.FormatModelErrors(ModelState)
                });
            }

            var result = await _serv.Create(form);

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            else if (result.code == 500)
                return StatusCode(result.code, new
                {
                    errorCode = "HB50001",
                    message = "Internal server error"
                });

            return StatusCode(result.code, new
            {
                Result = result.item
            });
        }

        [Authorize(Roles = "5,6")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileUpdate form)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    errorCode = "HB40001",
                    message = ValidationHelper.FormatModelErrors(ModelState)
                });
            }

            var result = await _serv.Update(id, form);

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            else if (result.code == 500)
                return StatusCode(result.code, new
                {
                    errorCode = "HB50001",
                    message = "Internal server error"
                });

            return StatusCode(result.code, new
            {
                Result = result.item
            });
        }

        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serv.Delete(id);

            if (result == 404)
                return StatusCode(result, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            else if (result == 500)
                return StatusCode(result, new
                {
                    errorCode = "HB50001",
                    message = "Internal server error"
                });


            return StatusCode(200, $"Deleted item id {id}");
        }
    }
}
