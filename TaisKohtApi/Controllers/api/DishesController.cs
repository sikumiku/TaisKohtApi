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
    [Route("api/v1/Dishes")]
    public class DishesController : Controller
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Gets all dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes
        [HttpGet]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_dishService.GetAllDishes());
        }

        /// <summary>
        /// Find dish by ID
        /// </summary>
        /// <param name="id">ID of dish to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Dish not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/5
        [HttpGet("{id}", Name = "GetDish")]
        [ProducesResponseType(typeof(DishDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var p = _dishService.GetDishById(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        /// <summary>
        /// Creates a dish
        /// </summary>
        /// <param name="dish">Dish object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Dishes
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created dish</returns>
        /// <response code="201">Returns the newly created dish</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Dishes
        [HttpPost]
        [ProducesResponseType(typeof(DishDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]DishDTO dish)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newDish = _dishService.AddNewDish(dish);

            return CreatedAtAction("GetDish", new { id = newDish.DishId }, newDish);
        }

        /// <summary>
        /// Update an existing promotion
        /// </summary>
        /// <param name="id">ID of promotion to update</param>
        /// <param name="person">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Promotions/{id}
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
        // PUT: api/v1/Dishes/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]DishDTO dish)
        {
            if (!ModelState.IsValid) return BadRequest();
            var p = _dishService.GetDishById(id);

            if (p == null) return NotFound();
            _dishService.UpdateDish(id, dish);

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
            var p = _dishService.GetDishById(id);
            if (p == null) return NotFound();
            _dishService.DeleteDish(id);
            return NoContent();
        }
    }
}