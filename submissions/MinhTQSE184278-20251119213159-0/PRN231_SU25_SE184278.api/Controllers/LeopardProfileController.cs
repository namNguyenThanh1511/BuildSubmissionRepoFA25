using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE184278.services;
using PRN231_SU25_SE184278.services.DTOs;

namespace PRN231_SU25_SE184278.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        private IActionResult UnauthorizedError()
            => Unauthorized(new ErrorResponseDto { ErrorCode = "HB40101", Message = "Token missing/invalid" });

        private IActionResult ForbiddenError()
            => StatusCode(403, new ErrorResponseDto { ErrorCode = "HB40301", Message = "Permission denied" });

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") &&
                !User.IsInRole("6") &&
                !User.IsInRole("7") &&
                !User.IsInRole("4"))
                return ForbiddenError();

            var result = await _service.GetAllAsync();
            return Ok(result); // 200
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") &&
                !User.IsInRole("6") &&
                !User.IsInRole("7") &&
                !User.IsInRole("4"))
                return ForbiddenError();

            var handbag = await _service.GetByIdAsync(id);
            if (handbag == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "LeopardProfile not found" });

            return Ok(handbag); // 200
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeopardProfileCreateDto dto)
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") && !User.IsInRole("6"))
                return ForbiddenError();


            var (success, error) = await _service.CreateAsync(dto);
            if (!success)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = error!.ErrorCode,
                    Message = error.Message
                });
            }

            return Created(string.Empty, new { Message = "LeopardProfile created successfully" });
        }


        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileCreateDto dto)
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") && !User.IsInRole("6"))
                return ForbiddenError();

            var handbag = await _service.GetByIdAsync(id);
            if (handbag == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "LeopardProfile not found" });
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

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") && !User.IsInRole("6"))
                return ForbiddenError();

            var handbag = await _service.GetByIdAsync(id);
            if (handbag == null)
                return NotFound(new ErrorResponseDto { ErrorCode = "HB40401", Message = "LeopardProfile not found" });

            var (success, error) = await _service.DeleteAsync(id);
            if (!success)
            {
                if (error?.ErrorCode == "HB40401")
                    return NotFound(error);

                return BadRequest(error);
            }

            return Ok(); // 200
        }

       
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? LeopardName, [FromQuery] double? Weight)
        {
            if (!User.Identity.IsAuthenticated)
                return UnauthorizedError();

            if (!User.IsInRole("5") &&
                !User.IsInRole("6") &&
                !User.IsInRole("7") &&
                !User.IsInRole("4"))
                return ForbiddenError();

            var res = await _service.SearchAsync(LeopardName, Weight);
            return Ok(res);
        }
    }
}
