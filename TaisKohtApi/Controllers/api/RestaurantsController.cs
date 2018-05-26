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
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public RestaurantsController(IRestaurantService restaurantService, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _restaurantService = restaurantService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all restaurants as a list
        /// </summary>
        /// <response code="200">Successful restaurant</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_restaurantService.GetAllRestaurants());
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
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
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
        /// Gets top restaurants as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants/Top
        [AllowAnonymous]
        [HttpGet("Top")]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Top(int amount)
        {
            var result = _restaurantService.GetTopRestaurants(amount);
            if (!result.Any())
            {
                return NotFound();
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
        /// Add new user to restaurant's users list
        /// </summary>
        /// <param name="id">ID of restaurant that you wanna add new user to</param>
        /// <param name="userId">ID of user that you wanna add to restaurant</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Restaurant or user not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Restaurants/5
        [AllowAnonymous]
        [HttpPost]
        [Route("addUserToRestaurant")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult AddUserToRestaurant([FromQuery(Name = "id")] int id, [FromQuery(Name = "userId")] string userId)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);
            var user = _userManager.FindByIdAsync(userId);
            if (restaurant == null || user == null) return NotFound();
            if (!IsAuthorized(restaurant)) { return StatusCode(403, "You have to be logged in as one of the restaurant users to add new users to restaurant."); }
            var users = _restaurantService.GetRestaurantUsersById(restaurant.RestaurantId);
            var userIds = new ArrayList();
            users.ForEach(u => userIds.Add(u.UserId));
            if (userIds.Contains(userId)) { return BadRequest("Provided user is already user of this restaurant."); }

            _restaurantService.AddUserToRestaurant(id, userId);
            return StatusCode(201);
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
        [Authorize(Roles = "admin, normalUser, premiumUser")] // replace with [AllowAnonymous] while user generation is broken
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]PostRestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (restaurantDTO.UserId != User.Identity.GetUserId()) return BadRequest("Provided userId does not match logged in user Id");

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
        public IActionResult Put(int id, [FromBody]PostRestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var restaurant = _restaurantService.GetRestaurantById(id);

            if (restaurant == null) return NotFound();
            if (IsAuthorized(restaurant))
            {
                _restaurantService.UpdateRestaurant(id, restaurantDTO);
            }
            else
            {
                return StatusCode(403, "This action is forbidden to unauthorized user.");
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
            users.ForEach(u => userIds.Add(u.UserId));
            return User.IsInRole("admin") || userIds.Contains(User.Identity.GetUserId());
        }
    }
}