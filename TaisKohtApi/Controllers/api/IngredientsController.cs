using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaisKohtApi.Controllers.api
{
    [Produces("application/json")]
    [Route("api/v1/Ingredients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //spetsifitseerime authenticationschemei, siis ei toimu reroutingut
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
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<IngredientDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
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
        [HttpGet("{id}", Name = "GetIngredient")]
        [ProducesResponseType(typeof(IngredientDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var i = _ingredientService.GetIngredientById(id);
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
        [HttpPost]
        [ProducesResponseType(typeof(IngredientDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]IngredientDTO ingredientDTO)
        {
            //ingredientDTO.UserId; //objekti k2est v6tta id, ManyToMany puhul kuidas teha?
            //User.Identity.GetUserId(); //id saamine useri k2est
            if (!ModelState.IsValid) return BadRequest();

            var newIngredient = _ingredientService.AddNewIngredient(ingredientDTO);

            return CreatedAtAction("GetIngredient", new { id = newIngredient.IngredientId }, newIngredient);
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
        /// <response code="204">Ingredient was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Ingredients/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]IngredientDTO ingredientDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var i = _ingredientService.GetIngredientById(id);

            if (i == null) return NotFound();
            _ingredientService.UpdateIngredient(id, ingredientDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a ingredient by id.
        /// </summary>
        /// <param name="id">ID of ingredient to delete</param>
        /// <response code="204">Ingredient was successfully deleted, no content to be returned</response>
        /// <response code="404">Ingredient not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Ingredients/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var i = _ingredientService.GetIngredientById(id);
            if (i == null) return NotFound();
            _ingredientService.DeleteIngredient(id);
            return NoContent();
        }
    }
}