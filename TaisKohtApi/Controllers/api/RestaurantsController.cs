using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpGet]
        [ProducesResponseType(typeof(List<RestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            List<String> types = new List<string>();
            foreach (var claim in User.Claims)
            {
                types.Add("Type:" + claim.Type);
                types.Add("Value:" + claim.Value);
            }
            //return Ok(_restaurantService.GetAllRestaurants());
            return Ok(types);
        }

        /// <summary>
        /// Gets searched restaurants as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants/search?name=th
        [AllowAnonymous]
        [HttpGet("Search")]
        [ProducesResponseType(typeof(List<RestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Search(string name)
        {
            var result = _restaurantService.SearchRestaurantByName(name);
            if (!result.Any())
            {
                return NotFound(name);
            }

            return Ok(result);
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
        [AllowAnonymous]
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
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]RestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newRestaurant = _restaurantService.AddNewRestaurant(restaurantDTO);

            return CreatedAtAction("Get", new { id = newRestaurant.RestaurantId }, newRestaurant);
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
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]RestaurantDTO restaurantDTO)
        {

            if (!ModelState.IsValid) return BadRequest();
            var restaurant = _restaurantService.GetRestaurantById(id);

            if (restaurant == null) return NotFound();
            if (IsAuthorized(restaurant))
            {
                _restaurantService.UpdateRestaurant(id, restaurantDTO);
            }
            else
            {
                return StatusCode(403, Json("This action is forbidden to unauthorized user."));
            }
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
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);
            if (restaurant == null) return NotFound();
            if (IsAuthorized(restaurant))
            {
                _restaurantService.DeleteRestaurant(id);
            }
            else
            {
                return StatusCode(403, Json("This action is forbidden to unauthorized user."));
            }
            return NoContent();
        }

        private Boolean IsAuthorized(RestaurantDTO restaurant)
        {
            var users = _restaurantService.GetRestaurantUsersById(restaurant.RestaurantId);
            var userIds = new ArrayList();
            users.ForEach(u => userIds.Add(u.Email));
            //Identity GetUserId returns user email not id
            return User.IsInRole("admin") || userIds.Contains(User.Identity.GetUserId());
        }
    }
}