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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //spetsifitseerime authenticationschemei, siis ei toimu reroutingut
    [Produces("application/json")]
    [Route("api/v1/ingredients")]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _ingredientService;
        private readonly IRequestLogService _requestLogService;

        public IngredientsController(IIngredientService ingredientService, IRequestLogService requestLogService)
        {
            _ingredientService = ingredientService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all user ingredients as a list (admin user gets all ingredients as a list).
        /// </summary>
        /// <returns>All user ingredients as a list (for admin user all ingredients as a list)</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no ingredients can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/ingredients
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpGet]
        [ProducesResponseType(typeof(List<IngredientDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllIngredientsCreatedByUser()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/ingredients", "GetAllIngredientsCreatedByUser");
            if (!User.IsInRole("admin")) return Ok(_ingredientService.GetAllUserIngredients(User.Identity.GetUserId()));

            return Ok(_ingredientService.GetAllIngredients());
        }

        /// <summary>
        /// Find user ingredient by ID (admin user can find all users ingredient by ID).
        /// </summary>
        /// <param name="id">ID of ingredient to return</param>
        /// <returns>Ingredient by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Ingredient not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/ingredients/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetIngredient")]
        [ProducesResponseType(typeof(IngredientDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetIngredient(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/ingredients/{id}", "GetIngredient");
            var ingredient = _ingredientService.GetIngredientById(id);
            if (ingredient == null) return NotFound();
            return Ok(ingredient);
        }

        /// <summary>
        /// Creates a ingredient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/ingredients
        ///     {
        ///         "Name": "Milk",
        ///         "Description": "Milk with 3% or more fat",
        ///         "AmountUnit" : "l"
        ///     }
        ///
        /// </remarks>
        /// <param name="ingredientDTO">PostIngredientDTO object to be added</param>
        /// <returns>A newly created ingredient</returns>
        /// <response code="201">Returns the newly created ingredient</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/ingredients
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost]
        [ProducesResponseType(typeof(IngredientDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostIngredient([FromBody]PostIngredientDTO ingredientDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/ingredients", "PostIngredient");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");

            var newIngredient = _ingredientService.AddNewIngredient(ingredientDTO, User.Identity.GetUserId());

            return CreatedAtAction(nameof(GetIngredient), new { id = newIngredient.IngredientId }, newIngredient);
        }

        /// <summary>
        /// Update an existing ingredient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/ingredients/{id}
        ///     {
        ///         "Name": "Milk",
        ///         "Description": "Milk with 2,5% fat",
        ///         "AmountUnit" : "l"
        ///     }
        ///
        /// </remarks>
        /// <param name="id" name="ingredientDTO">ID of ingredient to update and updated PostIngredientDTO object</param>
        /// <returns>Updated ingredient</returns>
        /// <response code="200">Ingredient was successfully updated, updated Ingredient to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/ingredients/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IngredientDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateIngredient(int id, [FromBody]PostIngredientDTO ingredientDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/ingredients", "UpdateIngredient");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var ingredient = _ingredientService.GetIngredientById(id);

            if (ingredient == null) return NotFound();

            if (ingredient.UserId != User.Identity.GetUserId()) { return StatusCode(403, "Ingredients can be amended only by admins or users that created them. Please provide id of ingredient that is created by user."); }

            IngredientDTO updatedIngredient = _ingredientService.UpdateIngredient(id, ingredientDTO);
            return Ok(updatedIngredient);
        }

        /// <summary>
        /// Deletes a ingredient by id.
        /// </summary>
        /// <param name="id">ID of ingredient to delete</param>
        /// <response code="204">Ingredient was successfully deleted, no content to be returned</response>
        /// <response code="404">Ingredient not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/ingredients/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteIngredient(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/ingredients", "DeleteIngredient");
            var ingredient = _ingredientService.GetIngredientById(id);
            if (ingredient == null) return NotFound();
            if (ingredient.UserId != User.Identity.GetUserId() && !User.IsInRole("admin")) return StatusCode(403, "Ingredient can only be deleted by admin or by logged in user who created the ingredient.");
            _ingredientService.DeleteIngredient(id);
            return NoContent();
        }
    }
}