using System;
using System.Collections;
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
    [Route("api/v1/dishes")]
    public class DishesController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IRestaurantService _restaurantService;
        private readonly IIngredientService _ingredientService;
        private readonly IRequestLogService _requestLogService;

        public DishesController(IDishService dishService, IRestaurantService restaurantService, IIngredientService ingredientService, IRequestLogService requestLogService)
        {
            _dishService = dishService;
            _restaurantService = restaurantService;
            _ingredientService = ingredientService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all dishes as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllDishes()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes", "GetAllDishes");
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
        [AllowAnonymous]
        [HttpGet]
        [Route("daily")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetDailyDishes([FromQuery(Name = "vegan")]bool vegan, [FromQuery(Name = "glutenFree")]bool glutenFree, [FromQuery(Name = "lactoseFree")]bool lactoseFree)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/daily", "GetDailyDishes");
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
        // GET: api/v1/Dishes/Search?title=th
        [AllowAnonymous]
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult SearchForDishes([FromQuery(Name = "title")]string title, [FromQuery(Name = "priceLimit")]decimal? priceLimit)
        {
            if (title != null)
            {
                _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/search", "SearchForDishesWithTitle");
                var result = _dishService.SearchDishByTitle(title);
                if (!result.Any())
                {
                    return NotFound("Can't find by this title '" + title + "'.");
                }

                return Ok(result);
            }

            if (priceLimit != null)
            {
                _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/search", "SearchForDishesWithPriceLimit");
                var result = _dishService.SearchDishByPriceLimit(priceLimit);
                if (!result.Any())
                {
                    return NotFound("Can't find by this price limit '" + priceLimit + "'.");
                }
                return Ok(result);
            }
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/search", "SearchForDishesWithoutParameters");

            return BadRequest("For searching you need to provide either title or price limit.");

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
        [HttpGet]
        [Route("top")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetTopDishes([FromQuery(Name = "amount")]int amount)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/top", "GetTopDishes");
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
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetDish")]
        [ProducesResponseType(typeof(DishDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetDish(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/dishes/{id}", "GetDish");
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            return Ok(dishDTO);
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
        [ProducesResponseType(typeof(DishDTO), 201)]
        [ProducesResponseType(typeof(PostDishDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostDish([FromBody]PostDishDTO dishDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/dishes", "PostDish");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (dishDTO.RestaurantId.Equals(null)) return BadRequest("Dish is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("New dish can only be added by admin or by restaurant user");
            if (!(User.IsInRole("premiumUser") || User.IsInRole("admin")) &&
                dishDTO.PromotionId != null)
                return BadRequest("New dish with promotion can only be added by admin or premium user");

            var newDish = _dishService.AddNewDish(dishDTO, User.Identity.GetUserId());

            return CreatedAtRoute("GetDish", new { id = newDish.DishId }, newDish);
        }

        /// <summary>
        /// Adds ingredient to dish
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PostIngredientForDishDTO"></param>
        /// <response code="204">Ingredients were successfully added to dish</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="404">Dish not found by given Id</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Dishes/addIngredient
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}/ingredients")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult AddIngredientsToDish(int id, [FromBody]PostIngredientForDishDTO[] dishes)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/dishes/{id}/ingredients", "AddIngredientsToDish");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("Dishes can only be added by admin or by restaurant user");

            _dishService.UpdateDishIngredients(id, dishes);

            return NoContent();
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
        /// <response code="200">Dish was successfully updated, returns updated dish</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Dishes/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DishDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateDish(int id, [FromBody]PostDishDTO dishDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/dishes/{id}", "UpdateDish");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (dishDTO.RestaurantId.Equals(null)) return BadRequest("Dish is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("Dish can only be updated by admin or by restaurant user");

            var d = _dishService.GetDishById(id);

            if (d == null) return NotFound();

            if (!(User.IsInRole("premiumUser") || User.IsInRole("admin")) &&
                dishDTO.PromotionId != null && dishDTO.PromotionId != d.PromotionId)
                return BadRequest("Promotions to dishes can only be added by admin or premium user");

            DishDTO updatedDish =_dishService.UpdateDish(id, dishDTO);

            return Ok(updatedDish);
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
        public IActionResult DeleteDish(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/dishes/{id}", "DeleteDish");
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("Dish can only be deleted by admin or by restaurant user");
            _dishService.DeleteDish(id);
            return NoContent();
        }

        private Boolean IsRestaurantUserOrAdmin(int restaurantId)
        {
            var users = _restaurantService.GetRestaurantUsersById(restaurantId);
            var userIds = new ArrayList();
            users.ForEach(u => userIds.Add(u.UserId));
            return User.IsInRole("admin") || userIds.Contains(User.Identity.GetUserId());
        }
    }
}