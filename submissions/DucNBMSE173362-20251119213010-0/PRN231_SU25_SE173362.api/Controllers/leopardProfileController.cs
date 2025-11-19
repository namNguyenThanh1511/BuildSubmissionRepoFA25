using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE173362.BLL;

namespace PRN231_SU25_SE173362.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    [Authorize(Roles = "4,5,6,7")]

    public class leopardProfileController : Controller
    {
        private readonly LeopardProfileService _service;
        private readonly LeopardTypeService _type;

        public leopardProfileController(LeopardProfileService service, LeopardTypeService type)
        {
            _service = service;
            _type = type;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult();
                }

                var result = await _service.GetLeopards();

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

                var result = await _service.GetLeopardById(id);

                if (result == null)
                {
                    return ErrorHelper.NotFoundResult("No leopard found");
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

                var type = await _type.GetTypeById(model.LeopardTypeId);

                if (type == null)
                {
                    return ErrorHelper.BadRequestResult("Type is not existed");
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

                var item = await _service.GetLeopardById(model.LeopardProfileId);

                if (item == null)
                {
                    return ErrorHelper.NotFoundResult("Leopard is not existed");
                }

                var brand = await _type.GetTypeById(model.LeopardTypeId);

                if (brand == null)
                {
                    return ErrorHelper.BadRequestResult("Type is not existed");
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

                var result = await _service.GetLeopardById(id);

                if (result == null)
                {
                    return ErrorHelper.NotFoundResult("No leopard found");
                }

                return Ok(await _service.Delete(id));
            }
            catch (Exception ex)
            {
                return ErrorHelper.InternalServerErrorResult("Internal server error");
            }

        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<IActionResult> Search()
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return ErrorHelper.UnauthorizedResult(); 
                }

                var result = await _service.GetLeopards();

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




    }
}
