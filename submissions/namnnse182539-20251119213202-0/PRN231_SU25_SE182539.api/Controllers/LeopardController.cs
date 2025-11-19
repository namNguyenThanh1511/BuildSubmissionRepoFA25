using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.DTO;
using Service.Interface;

namespace PRN231_SU25_SE182539.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeopardController : ControllerBase
    {
        private readonly ILeopardService _service;

        public LeopardController(ILeopardService service)
        {
            _service = service;
        }

        [HttpGet]
         
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var h = _service.GetById(id);
            if (h == null)
                return NotFound(new { errorCode = "HB40401", message = "LeopardProfile not found" });
            return Ok(h);
        }

        [HttpPost]

        public IActionResult Create([FromBody] LeopardDto dto)
        {
            var err = _service.Create(dto);
            if (err != null)
                return BadRequest(new { errorCode = "HB40001", message = err });

            return StatusCode(201, new { message = "LeopardProfile Create successfully" });
        }

        [HttpPut("{id}")]

        public IActionResult Update(int id, [FromBody] LeopardDto dto)
        {
            var result = _service.Update(id, dto);
            if (result == "not_found")
                return NotFound(new { errorCode = "HB40401", message = "LeopardProfile not found" });
            if (result != null)
                return BadRequest(new { errorCode = "HB40001", message = result });

            return Ok(new { message = "LeopardProfile Update successfully" });
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);
            if (!success)
                return NotFound(new { errorCode = "HB40401", message = "Leopard not found" });

            return Ok(new { message = "LeopardProfile delete successfully" });
        }

       
    }

}
