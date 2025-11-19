using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using Service.Helper;

namespace PRN231_SU25_SE184012.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController(LeopardProfileService profileService) : ControllerBase
    {
        private readonly LeopardProfileService _profileService =
            profileService ?? throw new ArgumentException(nameof(LeopardProfileService));

        /// <summary>
        /// Get all
        /// </summary>
        [HttpGet]
        [Authorize(
            Roles = ConstantsConfig.Admin
                + ","
                + ConstantsConfig.Mod
                + ","
                + ConstantsConfig.Dev
                + ","
                + ConstantsConfig.Mem
        )]

        public async Task<IActionResult> GetAllHandbags()
        {
            var result = await _profileService.GetAllHandbagsAsync();

            if (result != null)
                return Ok(result);

            return BadRequest(
                new ErrorModel(
                    ConstantsConfig.ErrorMessages.BadRequest,
                    ConstantsConfig.ErrorCodes.BadRequest
                )
            );
        }

        /// <summary>
        /// Get by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(
            Roles = ConstantsConfig.Admin
                + ","
                + ConstantsConfig.Mod
                + ","
                + ConstantsConfig.Dev
                + ","
                + ConstantsConfig.Mem
        )]
        public async Task<IActionResult> GetHandbagById(int id)
        {
            try
            {
                var result = await _profileService.GetProfileByIdAsync(id);
                if (result == null)
                {
                    var error = new ErrorModel(
                        ConstantsConfig.ErrorMessages.NotFound,
                        ConstantsConfig.ErrorCodes.NotFound
                    );
                    return NotFound(error);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.InternalServer,
                        ConstantsConfig.ErrorCodes.InternalServer
                    )
                );
            }
        }

        /// <summary>
        /// Create new
        /// </summary>
        [HttpPost]
        [Authorize(Roles = ConstantsConfig.Admin + "," + ConstantsConfig.Mod)]
        public async Task<IActionResult> CreateProfile([FromBody] LeopardCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        new ErrorModel(
                            ConstantsConfig.ErrorMessages.BadRequest,
                            ConstantsConfig.ErrorCodes.BadRequest
                        )
                    );
                }

                var handbag = await _profileService.CreateProfileAsync(request);
                if (handbag == null)
                {
                    return BadRequest(
                        new ErrorModel(
                            ConstantsConfig.ErrorMessages.BadRequest,
                            ConstantsConfig.ErrorCodes.BadRequest
                        )
                    );
                }
                return CreatedAtAction(
                    nameof(GetHandbagById),
                    new { id = handbag.LeopardProfileId },
                    handbag
                );
            }
            catch (Exception)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.InternalServer,
                        ConstantsConfig.ErrorCodes.InternalServer
                    )
                );
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = ConstantsConfig.Admin + "," + ConstantsConfig.Mod)]

        public async Task<IActionResult> UpdateProfile(
            int id,
            [FromBody] LeopardUpdateRequest request
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.BadRequest,
                        ConstantsConfig.ErrorCodes.BadRequest
                    )
                );
            }

            if (id <= 0 || request == null)
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.BadRequest,
                        ConstantsConfig.ErrorCodes.BadRequest
                    )
                );

            try
            {
                var result = await _profileService.UpdateProfileAsync(id, request);

                if (result != null)
                    return Ok(result);

                return NotFound(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.NotFound,
                        ConstantsConfig.ErrorCodes.NotFound
                    )
                );
            }
            catch (Exception)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.InternalServer,
                        ConstantsConfig.ErrorCodes.InternalServer
                    )
                );
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = ConstantsConfig.Admin + "," + ConstantsConfig.Mod)]

        public async Task<IActionResult> DeleteProfile(int id)
        {
            if (id <= 0)
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.BadRequest,
                        ConstantsConfig.ErrorCodes.BadRequest
                    )
                );

            var result = await _profileService.DeleteProfileAsync(id);

            if (result)
                return Ok();

            return NotFound(
                new ErrorModel(
                    ConstantsConfig.ErrorMessages.NotFound,
                    ConstantsConfig.ErrorCodes.NotFound
                )
            );
        }

        /// <summary>
        /// Search handbags with filters
        /// </summary>
        [HttpGet("search")]
        [EnableQuery]
        // [EnableQuery(
        //     AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Select,
        //     MaxTop = 100,
        //     MaxSkip = 1000,
        //     MaxOrderByNodeCount = 5,
        //     AllowedOrderByProperties = "ModelName,Price,ReleaseDate",
        //     AllowedArithmeticOperators = AllowedArithmeticOperators.None,
        //     AllowedLogicalOperators = AllowedLogicalOperators.And | AllowedLogicalOperators.Or | AllowedLogicalOperators.Equal | AllowedLogicalOperators.NotEqual | AllowedLogicalOperators.GreaterThan | AllowedLogicalOperators.LessThan,
        //     AllowedFunctions = AllowedFunctions.Contains | AllowedFunctions.StartsWith | AllowedFunctions.EndsWith
        // )]
        [Authorize(
            Roles = ConstantsConfig.Admin
                + ","
                + ConstantsConfig.Mod
                + ","
                + ConstantsConfig.Dev
                + ","
                + ConstantsConfig.Mem
        )]

        public IActionResult SearchProfiles([FromQuery] ProfileQueryRequest request)
        {
            try
            {
                var result = _profileService.GetProfilesQueryableByQuery(request);

                if (result != null)
                    return Ok(result.ToList());

                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.BadRequest,
                        ConstantsConfig.ErrorCodes.BadRequest
                    )
                );
            }
            catch (Exception)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.InternalServer,
                        ConstantsConfig.ErrorCodes.InternalServer
                    )
                );
            }
        }


    }
}
