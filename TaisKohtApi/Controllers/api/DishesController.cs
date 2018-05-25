using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
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
        /// Gets today daily dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Daily
        [HttpGet("Daily")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Daily(bool vegan, bool glutenFree, bool lactoseFree)
        {
            var result = _dishService.GetAllDailyDishes(vegan, glutenFree, lactoseFree);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets searched dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/search?title=th
        [HttpGet("Search")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Search(string title)
        {
            var result = _dishService.SearchDishByTitle(title);
            if (!result.Any())
            {
                return NotFound(title);
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets price limited dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Pricelimit
        [HttpGet("Pricelimit")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Pricelimit(decimal priceLimit)
        {
            var result = _dishService.SearchDishByPriceLimit(priceLimit);
            if (!result.Any())
            {
                return NotFound(priceLimit);
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets top dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Top
        [AllowAnonymous]
        [HttpGet("Top")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Top(int amount)
        {
            var result = _dishService.GetTopDishes(amount);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
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
            var d = _dishService.GetDishById(id);
            if (d == null) return NotFound();
            return Ok(d);
        }

        /// <summary>
        /// Creates a dish
        /// </summary>
        /// <param name="dishDTO">Dish object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Dishes
        ///     {
        ///         "title" : "Chicken Kiev",
        ///         "description" : "Tasty meal",
        ///         "vegan" : false,
        ///         "price" : 5.25,
        ///         "daily" : true,
        ///         "glutenFree" : true,
        ///         "restaurantId" : 4,
        ///         "userId" : 12
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created dish</returns>
        /// <response code="201">Returns the newly created dish</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Dishes
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost(Name = "PostDish")]
        [ProducesResponseType(typeof(PostDishDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]PostDishDTO dishDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newDish = _dishService.AddNewDish(dishDTO);

            return CreatedAtAction("Get", new { id = newDish.DishId }, newDish);
        }

        /// <summary>
        /// Update an existing dish
        /// </summary>
        /// <param name="id">ID of dish to update</param>
        /// <param name="PostDishDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Dishes/{id}
        ///     {
        ///         "title" : "Roast beef",
        ///         "description" : "Tasty meal",
        ///         "vegan" : false,
        ///         "price" : 7.5,
        ///         "daily" : false,
        ///         "glutenFree" : true,
        ///         "restaurantId" : 2,
        ///         "userId" : 5
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Dish was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Dishes/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]PostDishDTO dishDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var d = _dishService.GetDishById(id);

            if (d == null) return NotFound();
            _dishService.UpdateDish(id, dishDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a dish by id.
        /// </summary>
        /// <param name="id">ID of dish to delete</param>
        /// <response code="204">Dish was successfully deleted, no content to be returned</response>
        /// <response code="404">Dish not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Dishes/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var d = _dishService.GetDishById(id);
            if (d == null) return NotFound();
            _dishService.DeleteDish(id);
            return NoContent();
        }
    }
}