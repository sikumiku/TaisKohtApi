using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaisKohtApi.Controllers.api
{
    [Produces("application/json")]
    [Route("api/v1/Ratings")]
    public class RatingLogsController : Controller
    {
        private readonly IRatingLogService _ratingLogService;

        public RatingLogsController(IRatingLogService ratingLogService)
        {
            _ratingLogService = ratingLogService;
        }

        /// <summary>
        /// Gets all ratings as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no ratings can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Ratings
        [HttpGet]
        [ProducesResponseType(typeof(List<RatingLogDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_ratingLogService.GetAllRatingLogs());
        }

        /// <summary>
        /// Find rating by ID
        /// </summary>
        /// <param name="id">ID of rating to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Rating not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Ratings/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RatingLogDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var dto = _ratingLogService.GetRatingLogById(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Creates a rating
        /// </summary>
        /// <param name="ratingDTO">Rating object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Ratings
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created rating</returns>
        /// <response code="201">Returns the newly created rating</response>
        /// <response code="400">Rating object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Rating
        [HttpPost(Name = "PostRating")]
        [ProducesResponseType(typeof(RatingLogDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]RatingLogDTO ratingDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newRating = _ratingLogService.AddNewRatingLog(ratingDTO);

            return CreatedAtAction("PostRating", new { id = newRating.RatingLogId }, newRating);
        }

        /// <summary>
        /// Update an existing rating
        /// </summary>
        /// <param name="id">ID of rating to update</param>
        /// <param name="ratingDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Ratings/{id}
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Rating was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Ratings/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]RatingLogDTO ratingDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var dto = _ratingLogService.GetRatingLogById(id);

            if (dto == null) return NotFound();
            _ratingLogService.UpdateRatingLog(id, ratingDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a rating by id.
        /// </summary>
        /// <param name="id">ID of rating to delete</param>
        /// <response code="204">Rating was successfully deleted, no content to be returned</response>
        /// <response code="404">Rating not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Ratings/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var dto = _ratingLogService.GetRatingLogById(id);
            if (dto == null) return NotFound();
            _ratingLogService.DeleteRatingLog(id);
            return NoContent();
        }
    }
}