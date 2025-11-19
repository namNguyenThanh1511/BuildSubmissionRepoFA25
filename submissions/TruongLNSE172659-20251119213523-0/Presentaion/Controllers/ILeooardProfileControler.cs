using Bisiness.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentaion.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeooardProfileControler : ControllerBase
    {
        private readonly ILeooardProfile _leooardProfile;

        public LeooardProfileControler(ILeooardProfile leooardProfile)
        {
            _leooardProfile = leooardProfile;

        }

        [HttpGet]
        [Authorize(Roles = "4,5,6,7")]

        public async Task<IActionResult> Get()
        {

            var a = await _leooardProfile.getAllHandbags();
            return Ok(a);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4,5,6,7")]
        public async Task<IActionResult> GetById(int id)
        {

            var temp = await _leooardProfile.GetbyId(id);
            return Ok(temp);
        }

        [HttpPost]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Create(int id)
        {

            return Ok("");
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Delete(int id)
        {

            var temp = await _leooardProfile.GetbyId(id);
            return Ok(temp);
        }
    }
}
