using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/menus")]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IRestaurantService _restaurantService;
        private readonly IRequestLogService _requestLogService;

        public MenusController(IMenuService menuService, IRestaurantService restaurantService, IRequestLogService requestLogService)
        {
            _menuService = menuService;
            _restaurantService = restaurantService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Gets all menus as a list, only accessible as admin
        /// </summary>
        /// <returns>All menus as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no menus can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/menus
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<MenuDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllMenus()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/menus", "GetAllMenus");
            return Ok(_menuService.GetAllMenus());
        }

        /// <summary>
        /// Gets all user menus as a list.
        /// </summary>
        /// <returns>All user menus as a list</returns>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no menus can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/menus/owner
        [Authorize(Roles = "normalUser, premiumUser")]
        [HttpGet]
        [Route("owner")]
        [ProducesResponseType(typeof(List<SimpleMenuDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetAllMenusByUser()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/menus/owner", "GetAllMenusByUser");
            var result = _menuService.GetAllMenusByUser(User.Identity.GetUserId());
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Find menu by ID.
        /// </summary>
        /// <param name="id">ID of menu to return</param>
        /// <returns>Menu by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Menu not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/menus/5
        [AllowAnonymous]
        [HttpGet("{id}", Name="GetMenu")]
        [ProducesResponseType(typeof(MenuDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetMenu(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/menus/{id}", "GetMenu");
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();
            return Ok(menuDTO);
        }

        /// <summary>
        /// Creates a menu.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/menus
        ///     {
        ///         "Name": "Special menu",
        ///         "ActiveFrom": "2018-05-27T20:47:45.751Z",
        ///         "ActiveTo": "2018-05-27T20:47:45.751Z",
        ///         "RepetitionInterval": 7,
        ///         "restaurantId": 1,
        ///         "promotionId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="menuDTO">PostMenuDTO object to be added</param>
        /// <returns>A newly created menu</returns>
        /// <response code="201">Returns the newly created menu</response>
        /// <response code="400">Menu object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/menus
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost(Name = "PostMenu")]
        [ProducesResponseType(typeof(MenuDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult PostMenu([FromBody]PostMenuDTO menuDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/menus", "PostMenu");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (menuDTO.RestaurantId.Equals(null)) return BadRequest("Menu is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("New menu can only be added by admin or by restaurant user");

            int userMenus = _menuService.GetUserMenuCount(User.Identity.GetUserId());
            if (!User.IsInRole("premiumUser") && !User.IsInRole("admin"))
            {
                if (userMenus >= 1)
                    return StatusCode(403, "Regular user can only create 1 menu. Please sign up for premium services to add more.");

                if (menuDTO.PromotionId != null)
                    return StatusCode(403, "New menu with promotion can only be added by admin or premium user");
            }
            var newMenu = _menuService.AddNewMenu(menuDTO, User.Identity.GetUserId());

            return CreatedAtAction(nameof(GetMenu), new { id = newMenu.MenuId }, newMenu);
        }

        /// <summary>
        /// Update an existing menu.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/menus/{id}
        ///     {
        ///         "Name": "June menu",
        ///         "ActiveFrom": "2018-06-01T20:47:45.751Z",
        ///         "ActiveTo": "2018-06-30T20:47:45.751Z",
        ///         "RepetitionInterval": 1,
        ///         "restaurantId": 1,
        ///         "promotionId": null
        ///     }
        ///
        /// </remarks>
        /// <param name="id" name="menuDTO">ID of menu to update and updated PostMenuDTO object</param>
        /// <returns>Updated menu</returns>
        /// <response code="200">Menu was successfully updated, updated Menu to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Menus/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MenuDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateMenu(int id, [FromBody]PostMenuDTO menuDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/menus/{id}", "UpdateMenu");
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (menuDTO.RestaurantId.Equals(null)) return BadRequest("Menu is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("Menu can only be updated by admin or by restaurant user");

            var m = _menuService.GetMenuById(id);

            if (m == null) return NotFound();

            if (!(User.IsInRole("premiumUser") && !User.IsInRole("admin")) &&
                menuDTO.PromotionId != null && menuDTO.PromotionId != m.PromotionId)
                return StatusCode(403, "Promotions to menu can only be added by admin or premium user");

            MenuDTO updatedMenu = _menuService.UpdateMenu(id, menuDTO);

            return Ok(updatedMenu);
        }

        /// <summary>
        /// Deletes a menu by id.
        /// </summary>
        /// <param name="id">ID of menu to delete</param>
        /// <response code="204">Menu was successfully deleted, no content to be returned</response>
        /// <response code="404">Menu not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/menus/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteMenu(int id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/menus/{id}", "DeleteMenu");
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return StatusCode(403, "Menu can only be deleted by admin or by restaurant user");
            _menuService.DeleteMenu(id);
            return NoContent();
        }

        /// <summary>
        /// Adds dishes to the menu by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/menus/{id}/Dishes
        ///     [1, 2, 3]
        ///
        /// </remarks>
        /// <param name="id" name="dishIds">MenuId and dishIds</param>
        /// <response code="204">MenuDishes were successfully updated</response>
        /// <response code="404">Menu not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/menus/5/dishes
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}/Dishes")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult AddDishesToMenu(int id, [FromBody]int[] dishIds)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/menus/{id}/dishes", "AddDishesToMenu");
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();

            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return StatusCode(403, "Dishes can only be added to menu by admin or by restaurant user");

            _menuService.UpdateMenuDishes(id, dishIds);

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