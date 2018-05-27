using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/ratings")]
    public class RatingLogsController : Controller
    {
        private readonly IRatingLogService _ratingLogService;
        private readonly IRequestLogService _requestLogService;

        public RatingLogsController(IRatingLogService ratingLogService, IRequestLogService requestLogService)
        {
            _ratingLogService = ratingLogService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all ratings as a list, only accessible to admins.
        /// </summary>
        /// <returns>All ratings as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no ratings can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/ratings
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<RatingLogDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllRatingLogs()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/ratingLogs", "GetAllRatingLogs");
            return Ok(_ratingLogService.GetAllRatingLogs());
        }

        /// <summary>
        /// Find rating by ID. Only accessbile to admins.
        /// </summary>
        /// <param name="id">ID of rating to return</param>
        /// <returns>Rating by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Rating not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/ratings/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}", Name = "GetRatingLog")]
        [ProducesResponseType(typeof(RatingLogDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetRatingLog(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/ratingLogs/{id}", "GetRatingLog");
            var dto = _ratingLogService.GetRatingLogById(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Creates a rating.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/ratings
        ///     {
        ///         "Rating" : 3,
        ///         "Comment" : "Tasty meal",
        ///         "RestaurantId" : null,
        ///         "DishId" : 1,
        ///         "UserId" : "5f8811f5-2a80-4a8d-891f-12282e185aea"
        ///     }
        /// </remarks>
        /// <param name="ratingDTO">Rating object to be added</param>
        /// <returns>A newly created rating</returns>
        /// <response code="201">Returns the newly created rating</response>
        /// <response code="400">Rating object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/ratings
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost(Name = "PostRatingLog")]
        [ProducesResponseType(typeof(RatingLogForEntityDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostRatingLog([FromBody]RatingLogForEntityDTO ratingDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/ratingLogs", "PostRatingLog");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");

            if (ratingDTO.RestaurantId != null)
            {
                var existingRestaurantRating = _ratingLogService.GetRestaurantRatingLog(ratingDTO.RestaurantId, User.Identity.GetUserId());
                if (existingRestaurantRating.Comment != null && existingRestaurantRating.Rating != null)
                {
                    return BadRequest("User has already given this restaurant a rating.");
                }
                var newRating = _ratingLogService.AddNewRatingLog(ratingDTO, User.Identity.GetUserId());
                return CreatedAtAction(nameof(GetRatingLog), new { id = newRating.RatingLogId }, newRating);
            }
            if (ratingDTO.DishId != null)
            {
                var existingDishRating = _ratingLogService.GetDishRatingLog(ratingDTO.DishId, User.Identity.GetUserId());
                if (existingDishRating.Comment != null && existingDishRating.Rating != null)
                {
                    return BadRequest("User has already given this dish a rating.");
                }
                var newRating = _ratingLogService.AddNewRatingLog(ratingDTO, User.Identity.GetUserId());
                return CreatedAtAction(nameof(GetRatingLog), new { id = newRating.RatingLogId }, newRating);
            }
            return BadRequest("No dish or restaurant id provided to rate.");
        }

        /// <summary>
        /// Update an existing rating.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/ratings/{id}
        ///     {
        ///         "Rating" : 5,
        ///         "Comment" : "Tasty meal",
        ///         "RestaurantId" : 1,
        ///         "DishId" : 1,
        ///         "UserId" : "5f8811f5-2a80-4a8d-891f-12282e185aea"
        ///     }
        ///
        /// </remarks>
        /// <param name="id" name="ratingDTO">ID of rating to update and Updated RatingLogForEntityDTO object</param>
        /// <returns>Updated rating</returns>
        /// <response code="200">Rating was successfully updated, updated Rating to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/ratings/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateRestaurantRatingLog(int id, [FromBody]RatingLogForEntityDTO ratingDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/ratingLogs/{id}", "UpdateRestaurantRatingLog");
            if (!ModelState.IsValid) return BadRequest();
            var ratingLog = _ratingLogService.GetRatingLogById(id);

            if (ratingLog == null) return NotFound("No existing rating found.");
            if (ratingLog.UserId != User.Identity.GetUserId())
            {
                return StatusCode(403, "Users can only update their own rating. Please provide ratingLog id that belongs to logged in user.");
            }
            RatingLogDTO updatedRating = _ratingLogService.UpdateRatingLog(id, ratingDTO);

            return Ok(updatedRating);
        }

        /// <summary>
        /// Deletes a rating by id.
        /// </summary>
        /// <param name="id">ID of rating to delete</param>
        /// <response code="204">Rating was successfully deleted, no content to be returned</response>
        /// <response code="404">Rating not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/ratings/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteRatingLog(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/ratingLogs/{id}", "DeleteRatingLog");
            var ratingLog = _ratingLogService.GetRatingLogById(id);
            if (ratingLog == null) return NotFound();
            if (ratingLog.UserId == User.Identity.GetUserId())
            {
                return StatusCode(403, "RatingLog can only be deleted by admin or by user that posted it.");
            }
            _ratingLogService.DeleteRatingLog(id);
            return NoContent();
        }
    }
}