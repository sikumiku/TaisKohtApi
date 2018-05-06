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
    [Route("api/v1/Promotions")]
    public class PromotionsController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        /// <summary>
        /// Gets all promotions as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no promotions can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Promotions
        [HttpGet]
        [ProducesResponseType(typeof(List<PromotionDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_promotionService.GetAllPromotions());
        }

        /// <summary>
        /// Find promotion by ID
        /// </summary>
        /// <param name="id">ID of promotion to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Promotion not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Promotions/5
        [HttpGet("{id}", Name = "GetPromotion")]
        [ProducesResponseType(typeof(PromotionDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var p = _promotionService.GetPromotionById(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        /// <summary>
        /// Creates a promotion
        /// </summary>
        /// <param name="promotionDTO">Promotion object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Promotions
        ///     {
        ///         "name": "Chicken Sandvich",
        ///         "description": "Tender chicken sandvich with Marinara sauce between",
        ///         "type": "daily",
        ///         "validTo": "12/05/2018"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created promotion</returns>
        /// <response code="201">Returns the newly created promotion</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Promotions
        [HttpPost]
        [ProducesResponseType(typeof(PromotionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]PromotionDTO promotionDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newPromotion = _promotionService.AddNewPromotion(promotionDTO);

            return CreatedAtAction("GetPromotion", new { id = newPromotion.PromotionId }, newPromotion);
        }

        /// <summary>
        /// Update an existing promotion
        /// </summary>
        /// <param name="id">ID of promotion to update</param>
        /// <param name="promotionDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Promotions/{id}
        ///     {
        ///         "name": "Pasta Carbonara",
        ///         "description": "Classic pasta Carbonara with bacon and cheese",
        ///         "type": "weekly",
        ///         "validTo": "24/05/2018"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Promotion was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Promotions/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]PromotionDTO promotionDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var p = _promotionService.GetPromotionById(id);

            if (p == null) return NotFound();
            _promotionService.UpdatePromotion(id, promotionDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a promotion by id.
        /// </summary>
        /// <param name="id">ID of promotion to delete</param>
        /// <response code="204">Promotion was successfully deleted, no content to be returned</response>
        /// <response code="404">Promotion not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Promotions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var p = _promotionService.GetPromotionById(id);
            if (p == null) return NotFound();
            _promotionService.DeletePromotion(id);
            return NoContent();
        }
    }
}