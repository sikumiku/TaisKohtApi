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
    [Route("api/v1/restaurants")]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IRequestLogService _requestLogService;

        public RestaurantsController(IRestaurantService restaurantService, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IRequestLogService requestLogService)
        {
            _restaurantService = restaurantService;
            _userManager = userManager;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all restaurants as a list.
        /// </summary>
        /// <returns>All restaurants as a list</returns>
        /// <response code="200">Successful restaurant</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/restaurants
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllRestaurants()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/restaurants", "GetAllRestaurants");
            return Ok(_restaurantService.GetAllRestaurants());
        }

        /// <summary>
        /// Gets all user restaurants as a list.
        /// </summary>
        /// <returns>All user restaurants as a list</returns>
        /// <response code="200">Successful restaurant</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/restaurants
        [Authorize(Roles = "normalUser, premiumUser")]
        [HttpGet]
        [Route("owner")]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllRestaurantsByUser()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/restaurants/owner", "GetAllRestaurantsByUser");
            var result = _restaurantService.GetAllRestaurantsByUser(User.Identity.GetUserId());
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets searched restaurants as a list.
        /// </summary>
        /// <param name="name">name of restaurant to return</param>
        /// <returns>All searched restaurants as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no searched restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/restaurants/search?name=th
        [AllowAnonymous]
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult SearchRestaurantByName(string name)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/restaurants/search", "SearchRestaurantByName");
            var result = _restaurantService.SearchRestaurantByName(name);
            if (!result.Any())
            {
                return NotFound(name);
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets top restaurants as a list.
        /// </summary>
        /// <param name="amount">How many top rated restaurants to return</param>
        /// <returns>All top rated restaurants as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no restaurants can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/restaurants/Top
        [AllowAnonymous]
        [HttpGet]
        [Route("top")]
        [ProducesResponseType(typeof(List<SimpleRestaurantDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetTopRestaurants(int amount)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/restaurants/top", "GetTopRestaurants");
            var result = _restaurantService.GetTopRestaurants(amount);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Find restaurant by ID.
        /// </summary>
        /// <param name="id">ID of restaurant to return</param>
        /// <returns>Restaurant by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Restaurant not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/restaurants/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetRestaurant")]
        [ProducesResponseType(typeof(RestaurantDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetRestaurant(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/restaurants/{id}", "GetRestaurant");
            var r = _restaurantService.GetRestaurantById(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        /// <summary>
        /// Add new user to restaurant's users list.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Restaurants/addUserToRestaurant?id=1&userId=5f8811f5-2a80-4a8d-891f-12282e185aea
        /// </remarks>
        /// <param name="id" name="userId">ID of restaurant that you wanna add new user to and ID of user that you wanna add to restaurant</param>
        /// <response code="201">Successful operation</response>
        /// <response code="403">No premission</response>
        /// <response code="404">Restaurant or user not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/restaurants/addUserToRestaurant?id=1&userId=5f8811f5-2a80-4a8d-891f-12282e185aea
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost]
        [Route("addUserToRestaurant")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult AddUserToRestaurant([FromQuery(Name = "id")] int id, [FromQuery(Name = "userId")] string userId)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/restaurants/addUserToRestaurant", "AddUserToRestaurant");
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
        /// Creates a restaurant.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Restaurants
        ///     {
        ///        "Name": "Chef",    
        ///        "Url": "http://chef.com",    
        ///        "ContactNumber": "8228723089984329",    
        ///        "Email": "chef@gmail.com",
        ///        "Address": {        
        ///            "addressFirstLine": "Pärnu mnt 2",        
        ///            "locality": "Tallinn",        
        ///            "postCode": "12345",        
        ///            "region": "Center",        
        ///            "country": "Estonia"
        ///            }
        ///     }
        /// </remarks>
        /// <param name="restaurantDTO">PostRestaurantDTO object to be added</param>
        /// <returns>A newly created restaurant</returns>
        /// <response code="201">Returns the newly created restaurant</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/restaurants
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostRestaurant([FromBody]PostRestaurantDTO restaurantDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/restaurants", "PostRestaurant");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");

            int userRestaurants = _restaurantService.GetUserRestaurantCount(User.Identity.GetUserId());
            if (!User.IsInRole("premiumUser") && !User.IsInRole("admin"))
            {
                if(userRestaurants >= 1)
                return BadRequest("Regular user can only create 1 Restaurant. Please sign up for premium services to add more.");

                if (restaurantDTO.PromotionId != null)
                    return BadRequest("New menu with promotion can only be added by admin or premium user");
            }

            var newRestaurant = _restaurantService.AddNewRestaurant(restaurantDTO, User.Identity.GetUserId());

            return CreatedAtAction(nameof(GetRestaurant), new { id = newRestaurant.RestaurantId }, newRestaurant);
        }

        /// <summary>
        /// Update an existing restaurant.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/restaurants/{id}
        ///     {
        ///        "Name": "Chef",    
        ///        "Url": "http://chef.com",    
        ///        "ContactNumber": "8228723089984329",    
        ///        "Email": "chef@gmail.com",
        ///        "Address": {        
        ///            "addressFirstLine": "Pärnu mnt 12",        
        ///            "locality": "Tallinn",        
        ///            "postCode": "12345",        
        ///            "region": "Center",        
        ///            "country": "Estonia"
        ///            }
        ///     }
        /// </remarks>
        /// <param name="id" name="restaurantDTO">ID of restaurant to update and updated PostRestaurantDTO object</param>
        /// <returns>Updated restaurant</returns>
        /// <response code="200">Restaurant was successfully updated, updated Restaurant to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/restaurants/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RestaurantDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateRestaurant(int id, [FromBody]PostRestaurantDTO restaurantDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/restaurants/{id}", "UpdateRestaurant");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var restaurant = _restaurantService.GetRestaurantById(id);

            if (restaurant == null) return NotFound();

            if (!IsAuthorized(restaurant))
                return StatusCode(403, "You have to be logged in as one of the restaurant users to update restaurant information.");

            if (!(User.IsInRole("premiumUser") || User.IsInRole("admin")) &&
                restaurantDTO.PromotionId != null && restaurantDTO.PromotionId != restaurant.PromotionId)
                return StatusCode(403, "Promotions to restaurant can only be added by admin or premium user");

            return Ok(_restaurantService.UpdateRestaurant(id, restaurantDTO));
        }

        /// <summary>
        /// Deletes a restaurant by id.
        /// </summary>
        /// <param name="id">ID of restaurant to delete</param>
        /// <response code="204">Restaurant was successfully deleted, no content to be returned</response>
        /// <response code="404">Restaurant not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/restaurants/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteRestaurant(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/restaurants/{id}", "DeleteRestaurant");
            var restaurant = _restaurantService.GetRestaurantById(id);
            if (restaurant == null) return NotFound();
            if (IsAuthorized(restaurant))
            {
                _restaurantService.DeleteRestaurant(id);
            }
            else
            {
                return StatusCode(403, "This action is forbidden to unauthorized user.");
            }
            return NoContent();
        }

        /// <summary>
        /// Checks that logged in user is in role admin or one of reastaurant users.
        /// </summary>
        /// <param name="restaurant">RestaurantDTO</param>
        /// <returns>true, if user is in role admin or one of reastaurant users</returns>
        private Boolean IsAuthorized(RestaurantDTO restaurant)
        {
            var users = _restaurantService.GetRestaurantUsersById(restaurant.RestaurantId);
            var userIds = new ArrayList();
            users.ForEach(u => userIds.Add(u.UserId));
            return User.IsInRole("admin") || userIds.Contains(User.Identity.GetUserId());
        }
    }
}