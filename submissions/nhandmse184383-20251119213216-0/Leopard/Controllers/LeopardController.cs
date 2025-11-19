using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leopard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardController(ILeopardProfileService service)
        {
            _service = service;
        }

        // GET /api/handbags
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _service.GetAllAsync();
            return Ok(result); // 200
        }

        // GET /api/handbags/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var leopard = await _service.GetByIdAsync(id);
            if (leopard == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "Leopard not found" });

            return Ok(leopard); // 200
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeopardProfileCreateDto dto)
        {

            var (success, error) = await _service.CreateAsync(dto);
            if (!success)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = error!.ErrorCode,
                    Message = error.Message
                });
            }

            return Created(string.Empty, new { Message = "Leopard created successfully" });
        }


        // PUT /api/handbags/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileCreateDto dto)
        {

            var handbag = await _service.GetByIdAsync(id);
            if (handbag == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "Leopard not found" });
            var (success, error) = await _service.UpdateAsync(id, dto);

            if (!success)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = error!.ErrorCode,
                    Message = error.Message
                });
            }

            return Ok(); // 200
        }

        // DELETE /api/handbags/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var leopard = await _service.GetByIdAsync(id);
            if (leopard == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "Leopard not found" });

            var (success, error) = await _service.DeleteAsync(id);
            if (!success)
            {
                if (error?.ErrorCode == "HB40401")
                    return NotFound(error);

                return BadRequest(error);
            }

            return Ok(); // 200
        }
    }
}
