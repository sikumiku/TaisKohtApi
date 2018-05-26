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
    [Route("api/v1/Ingredients")]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Gets all ingredients as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no ingredients can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Ingredients
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<IngredientDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            if (!User.IsInRole("admin")) return Ok(_ingredientService.GetAllUserIngredients(User.Identity.GetUserId()));

            return Ok(_ingredientService.GetAllIngredients());
        }

        /// <summary>
        /// Find ingredient by ID
        /// </summary>
        /// <param name="id">ID of ingredient to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Ingredient not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Ingredients/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetIngredient")]
        [ProducesResponseType(typeof(IngredientDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetIngredient(int id)
        {
            var i = _ingredientService.GetIngredientById(id);
            if (i.UserId != User.Identity.GetUserId() && !User.IsInRole("admin")) return BadRequest("Ingredient can only be showed by admin or by logged in user who created the ingredient.");
            if (i == null) return NotFound();
            return Ok(i);
        }

        /// <summary>
        /// Creates a ingredient
        /// </summary>
        /// <param name="ingredientDTO">Ingredient object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Ingredients
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created ingredient</returns>
        /// <response code="201">Returns the newly created ingredient</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Ingredients
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost]
        [ProducesResponseType(typeof(IngredientDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]PostIngredientDTO ingredientDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");

            var newIngredient = _ingredientService.AddNewIngredient(ingredientDTO, User.Identity.GetUserId());

            return CreatedAtRoute("GetIngredient", new { id = newIngredient.IngredientId }, newIngredient);
        }

        /// <summary>
        /// Update an existing ingredient
        /// </summary>
        /// <param name="id">ID of ingredient to update</param>
        /// <param name="ingredientDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Ingredients/{id}
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Ingredient was successfully updated, updated Ingredient to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Ingredients/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IngredientDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]PostIngredientDTO ingredientDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var i = _ingredientService.GetIngredientById(id);

            if (i == null) return NotFound();

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
        // DELETE: api/v1/Ingredients/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var ingredient = _ingredientService.GetIngredientById(id);
            if (ingredient == null) return NotFound();
            if (ingredient.UserId != User.Identity.GetUserId() && !User.IsInRole("admin")) return BadRequest("Ingredient can only be deleted by admin or by logged in user who created the ingredient.");
            _ingredientService.DeleteIngredient(id);
            return NoContent();
        }
    }
}