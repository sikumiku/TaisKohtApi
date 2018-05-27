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
    [Route("api/v1/Dishes")]
    public class DishesController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IRestaurantService _restaurantService;
        private readonly IIngredientService _ingredientService;

        public DishesController(IDishService dishService, IRestaurantService restaurantService, IIngredientService ingredientService)
        {
            _dishService = dishService;
            _restaurantService = restaurantService;
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Gets all dishes as a list.
        /// </summary>
        /// <returns>All dishes as a list</returns>
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
        public IActionResult Get()
        {
            return Ok(_dishService.GetAllDishes());
        }

        /// <summary>
        /// Gets today daily dishes as a list.
        /// </summary>
        /// <param name="vegan" name="glutenFree" name="lactoseFree">parameters of dish to return</param>
        /// <returns>All today daily dishes as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no today daily dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Daily
        [AllowAnonymous]
        [HttpGet("Daily")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetDailyDishes(bool vegan, bool glutenFree, bool lactoseFree)
        {
            var result = _dishService.GetAllDailyDishes(vegan, glutenFree, lactoseFree);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets searched dishes as a list.
        /// </summary>
        /// <param name="title">title of dish to return</param>
        /// <returns>All searched dishes as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no searched dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Search?title=th
        [AllowAnonymous]
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
        /// Gets price limited dishes as a list.
        /// </summary>
        /// <param name="priceLimit">pricelimit of dish to return</param>
        /// <returns>All price limited dishes as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no price limited dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Pricelimit
        [AllowAnonymous]
        [HttpGet("Pricelimit")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PriceLimit(decimal priceLimit)
        {
            var result = _dishService.SearchDishByPriceLimit(priceLimit);
            if (!result.Any())
            {
                return NotFound(priceLimit);
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets top dishes as a list.
        /// </summary>
        /// <param name="amount">How many top rated dishes to return</param>
        /// <returns>All top rated dishes as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no top dishes can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Dishes/Top
        [AllowAnonymous]
        [HttpGet("Top")]
        [ProducesResponseType(typeof(List<DishDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetTopDishes(int amount)
        {
            var result = _dishService.GetTopDishes(amount);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Find dish by ID.
        /// </summary>
        /// <param name="id">ID of dish to return</param>
        /// <returns>Dish by ID</returns>
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
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            return Ok(dishDTO);
        }

        /// <summary>
        /// Creates a dish.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Dishes
        ///     {
        ///         "Title" : "Chicken Kiev",
        ///         "Description" : "Tasty meal",
        ///         "AvailableFrom" : "2018-05-27T20:51:22.508Z",
        ///         "AvailableTo" : "2018-05-27T20:51:22.508Z",
        ///         "ServeTime" : "2018-05-27T20:51:22.508Z",
        ///         "Vegan" : false,
        ///         "LactoseFree" : false,
        ///         "GlutenFree" : false,
        ///         "Kcal" : 400.00,
        ///         "WeightG" : 300.00,
        ///         "Price" : 7.25,
        ///         "DailyPrice" : 5.00,
        ///         "Daily" : true,
        ///         "RestaurantId" : 1,
        ///         "MenuId" : 1,
        ///         "PromotionId" : 1
        ///     }
        /// </remarks>
        /// <param name="dishDTO">PostDishDTO object to be added</param>
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
        public IActionResult Post([FromBody]PostDishDTO dishDTO)
        {
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
        /// Adds ingredients to dish.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Dishes/{id}/Ingredients
        ///     [{
        ///         "Amount" : "2.5",
        ///         "IngredientId" : 1
        ///         
        ///     },
        ///     
        ///     {
        ///         "Amount" : "7.5",
        ///         "IngredientId" : 2
        ///     },
        ///     
        ///     {
        ///         "Amount" : "5",
        ///         "IngredientId" : 3
        ///     }]
        ///
        /// </remarks>
        /// <param name="id" name="ingredients">DishId and ingredients</param>
        /// <response code="204">Ingredients were successfully added to dish</response>
        /// <response code="400">Provided object is faulty</response>
        /// <response code="404">Dish not found by given Id</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Dishes/{id}/Ingredients
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}/Ingredients")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult AddIngredientsToDish(int id, [FromBody]PostIngredientForDishDTO[] ingredients)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("Ingredients To Dish can only be added by admin or by restaurant user");

            _dishService.UpdateDishIngredients(id, ingredients);

            return NoContent();
        }

        /// <summary>
        /// Update an existing dish.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Dishes/{id}
        ///     {
        ///         "Title" : "Roast beef",
        ///         "Description" : "Tasty meal",
        ///         "AvailableFrom" : "2018-05-27T20:51:22.508Z",
        ///         "AvailableTo" : "2018-05-27T20:51:22.508Z",
        ///         "ServeTime" : "2018-05-27T20:51:22.508Z",
        ///         "Vegan" : false,
        ///         "LactoseFree" : false,
        ///         "GlutenFree" : false,
        ///         "Kcal" : 400.00,
        ///         "WeightG" : 300.00,
        ///         "Price" : 8.00,
        ///         "DailyPrice" : 5.00,
        ///         "Daily" : true,
        ///         "RestaurantId" : 1,
        ///         "MenuId" : 1,
        ///         "PromotionId" : 1
        ///     }
        /// </remarks>
        /// <param name="id" name="dishDTO">ID of dish to update and updated PostDishDTO object</param>
        /// <returns>Updated dish</returns>
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
        public IActionResult Put(int id, [FromBody]PostDishDTO dishDTO)
        {
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
        public IActionResult Delete(int id)
        {
            var dishDTO = _dishService.GetDishById(id);
            if (dishDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(dishDTO.RestaurantId)) return BadRequest("Dish can only be deleted by admin or by restaurant user");
            _dishService.DeleteDish(id);
            return NoContent();
        }

        /// <summary>
        /// Checks that logged in user is in role admin or one of reastaurant users.
        /// </summary>
        /// <param name="restaurantId">ID of restaurant</param>
        /// <returns>true, if user is in role admin or one of reastaurant users</returns>
        private Boolean IsRestaurantUserOrAdmin(int restaurantId)
        {
            var users = _restaurantService.GetRestaurantUsersById(restaurantId);
            var userIds = new ArrayList();
            users.ForEach(u => userIds.Add(u.UserId));
            return User.IsInRole("admin") || userIds.Contains(User.Identity.GetUserId());
        }
    }
}