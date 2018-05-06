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
    [Route("api/v1/Restaurants")]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Gets all restaurants as a list
        /// </summary>
        /// <response code="200">Successful restaurant</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants
        [HttpGet]
        [ProducesResponseType(typeof(List<RestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_restaurantService.GetAllRestaurants());
        }

        /// <summary>
        /// Find restaurant by ID
        /// </summary>
        /// <param name="id">ID of restaurant to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Restaurant not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants/5
        [HttpGet("{id}", Name = "GetRestaurant")]
        [ProducesResponseType(typeof(RestaurantDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var r = _restaurantService.GetRestaurantById(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        /// <summary>
        /// Creates a restaurant
        /// </summary>
        /// <param name="restaurantDTO">Restaurant object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Restaurants
        ///     {
        ///        ......
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created restaurant</returns>
        /// <response code="201">Returns the newly created restaurant</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Restaurants
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]RestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newRestaurant = _restaurantService.AddNewRestaurant(restaurantDTO);

            return CreatedAtAction("GetRestaurant", new { id = newRestaurant.RestaurantId }, newRestaurant);
        }

        /// <summary>
        /// Update an existing restaurant
        /// </summary>
        /// <param name="id">ID of restaurant to update</param>
        /// <param name="restaurantDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Restaurants/{id}
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Restaurant was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Restaurants/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]RestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var r = _restaurantService.GetRestaurantById(id);

            if (r == null) return NotFound();
            _restaurantService.UpdateRestaurant(id, restaurantDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a restaurant by id.
        /// </summary>
        /// <param name="id">ID of restaurant to delete</param>
        /// <response code="204">Restaurant was successfully deleted, no content to be returned</response>
        /// <response code="404">Restaurant not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/Restaurants/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var r = _restaurantService.GetRestaurantById(id);
            if (r == null) return NotFound();
            _restaurantService.DeleteRestaurant(id);
            return NoContent();
        }
    }
}