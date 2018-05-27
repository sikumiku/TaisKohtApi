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
    [Route("api/v1/Promotions")]
    public class PromotionsController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly IRequestLogService _requestLogService;

        public PromotionsController(IPromotionService promotionService, IRequestLogService requestLogService)
        {
            _promotionService = promotionService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all promotions as a list.
        /// </summary>
        /// <returns>All promotions as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no promotions can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Promotions
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<PromotionDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllPromotions()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/promotions", "GetAllPromotions");
            return Ok(_promotionService.GetAllPromotions());
        }

        /// <summary>
        /// Find promotion by ID.
        /// </summary>
        /// <param name="id">ID of promotion to return</param>
        /// <returns>Promotion by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Promotion not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Promotions/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetPromotion")]
        [ProducesResponseType(typeof(PromotionDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetPromotion(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/promotions/{id}", "GetPromotion");
            var promotionDTO = _promotionService.GetPromotionById(id);
            if (promotionDTO == null) return NotFound();
            return Ok(promotionDTO);
        }

        /// <summary>
        /// Creates a promotion.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Promotions
        ///     {
        ///         "Name" : "Chicken Sandvich",
        ///         "Description" : "Tender chicken sandvich with Marinara sauce between",
        ///         "Type": "daily",
        ///         "validTo": "2018-06-30T20:51:22.508Z",
        ///         "ClassName" : "Main"
        ///     }
        ///
        /// </remarks>
        /// <param name="promotionDTO">PromotionDTO object to be added</param>
        /// <returns>A newly created promotion</returns>
        /// <response code="201">Returns the newly created promotion</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Promotions
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(typeof(PromotionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostPromotion([FromBody]PromotionDTO promotionDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/promotions", "PostPromotion");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");

            var newPromotion = _promotionService.AddNewPromotion(promotionDTO);

            return CreatedAtRoute("GetPromotion", new { id = newPromotion.PromotionId }, newPromotion);
        }

        /// <summary>
        /// Update an existing promotion.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Promotions/{id}
        ///     {
        ///         "Name" : "Pasta Carbonara",
        ///         "Description" : "Classic pasta Carbonara with bacon and cheese",
        ///         "Type": "weekly",
        ///         "validTo": "2018-06-30T20:51:22.508Z",
        ///         "ClassName" : "Main"
        ///     }
        ///
        /// </remarks>
        /// <param name="id" name="promotionDTO">ID of promotion to update and Updated PromotionDTO object</param>
        /// <returns>Updated promotion</returns>
        /// <response code="200">Promotion was successfully updated, updated Promotion to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Promotions/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdatePromotion(int id, [FromBody]PromotionDTO promotionDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/promotions/{id}", "UpdatePromotion");
            if (!ModelState.IsValid) return BadRequest();
            var p = _promotionService.GetPromotionById(id);

            if (p == null) return NotFound();
            PromotionDTO updatedPromotion = _promotionService.UpdatePromotion(id, promotionDTO);

            return Ok(updatedPromotion);
        }

        /// <summary>
        /// Deletes a promotion by id.
        /// </summary>
        /// <param name="id">ID of promotion to delete</param>
        /// <response code="204">Promotion was successfully deleted, no content to be returned</response>
        /// <response code="404">Promotion not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Promotions/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeletePromotion(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/promotions/{id}", "DeletePromotion");
            var promotionDTO = _promotionService.GetPromotionById(id);
            if (promotionDTO == null) return NotFound();
            _promotionService.DeletePromotion(id);
            return NoContent();
        }
    }
}