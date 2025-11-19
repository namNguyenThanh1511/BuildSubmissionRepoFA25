using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173282.api.Error;
using PRN231_SU25_SE173282.BLL;

namespace PRN231_SU25_SE173282.api.Controllers
{
    
     [Route("api/leopards")]
     [ApiController]
    [Authorize(Roles = "1,2,3,4,5,6,7")]

    public class LeopardController : Controller
    {
        private readonly LeopardService _service;
        private readonly TypeService _brand;

        public LeopardController(LeopardService service, TypeService brand)
        {
            _service = service;
            _brand = brand;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult(); // Trả 401 + JSON chuẩn HB40101
                }
                if (!User.IsInRole("5") && !User.IsInRole("6") && !User.IsInRole("4") && !User.IsInRole("7"))
                {
                    return ErrorHelper.ForbiddenResult();
                }
                var result = await _service.GetHandbags();

                if (result == null)
                {
                    return ErrorHelper.NotFoundResult("No leopards found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult(); // Trả 401 + JSON chuẩn HB40101
                }
                if (!User.IsInRole("5") && !User.IsInRole("6") && !User.IsInRole("4") && !User.IsInRole("7"))
                {
                    return ErrorHelper.ForbiddenResult();
                }
                var result = await _service.GetHandbagById(id);

                if (result == null)
                {
                    return ErrorHelper.NotFoundResult("No handbag found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(LeopardCreateModel model)
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult();
                }

                if (!User.IsInRole("5") && !User.IsInRole("6"))
                {
                    return ErrorHelper.ForbiddenResult();
                }

                if (!ModelState.IsValid)
                {
                    return ErrorHelper.BadRequestResult(ModelState);
                }


                var result = await _service.Create(model);

                if (result != 1)
                {
                    return ErrorHelper.InternalServerErrorResult("Internal server error");
                }

                return StatusCode(StatusCodes.Status201Created, result);



            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update(LeopardUpdateModel model)
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult();
                }

                if (!User.IsInRole("5") && !User.IsInRole("6"))
                {
                    return ErrorHelper.ForbiddenResult();
                }

                if (!ModelState.IsValid)
                {
                    return ErrorHelper.BadRequestResult(ModelState);
                }

                var item = await _service.GetHandbagById(model.LeopardProfileId);

                if (item == null)
                {
                    return ErrorHelper.NotFoundResult("Hangbag is not existed");
                }


                return Ok(await _service.Update(model));


            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult();
                }

                if (!User.IsInRole("5") && !User.IsInRole("6"))
                {
                    return ErrorHelper.ForbiddenResult();
                }

                var result = await _service.GetHandbagById(id);

                if (result == null)
                {
                    return ErrorHelper.NotFoundResult("No handbag found");
                }

                return Ok(await _service.Delete(id));
            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpGet("search")]
        [Authorize(Roles = "4, 7")]

        public async Task<IActionResult> Search([FromQuery] string? modelName, [FromQuery] double? material)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return Unauthorized();
            if (!User.IsInRole("5") && !User.IsInRole("6") && !User.IsInRole("4") && !User.IsInRole("7"))
            {
                return ErrorHelper.ForbiddenResult();
            }

            var list = await _service.GetHandbags(); // List<Handbag>

            // Lọc thủ công nếu có tham số
            if (!string.IsNullOrWhiteSpace(modelName))
            {
                list = list.Where(h =>
                    h.LeopardName != null &&
                    h.LeopardName.Contains(modelName, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (!list.Any())
                return NotFound("No leopards found");

            // Group theo brand name và trả thông tin Handbag đầy đủ
            var grouped = list
                .GroupBy(h => h.LeopardType?.LeopardTypeName ?? "Unknown")
                .Select(g => new
                {
                    BrandName = g.Key,
                    Handbags = g.Select(h => new
                    {
                        h.LeopardProfileId,
                        h.LeopardName,
                        h.Weight,
                        h.Characteristics,
                        h.CareNeeds,
                        h.ModifiedDate
                    })
                });

            return Ok(grouped);
        }
    }
}
