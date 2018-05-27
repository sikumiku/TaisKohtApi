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
    [Route("api/v1/Menus")]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IRestaurantService _restaurantService;

        public MenusController(IMenuService menuService, IRestaurantService restaurantService)
        {
            _menuService = menuService;
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Gets all menus as a list
        /// </summary>
        /// <response code="200">Successful operation</response> 
        /// <response code="404">If no menus can be found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Menus
        [Obsolete("Get() is pointless. We are fetching menus associated with one restaurant. Can keep for sake of variety.")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<MenuDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return Ok(_menuService.GetAllMenus());
        }

        /// <summary>
        /// Find menu by ID
        /// </summary>
        /// <param name="id">ID of menu to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Menu not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/Menus/5
        [AllowAnonymous]
        [HttpGet("{id}", Name="GetMenu")]
        [ProducesResponseType(typeof(MenuDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetMenu(int id)
        {
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();
            return Ok(menuDTO);
        }

        /// <summary>
        /// Creates a menu
        /// </summary>
        /// <param name="menuDTO">Menu object to be added</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Menus
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created menu</returns>
        /// <response code="201">Returns the newly created menu</response>
        /// <response code="400">Menu object is faulty</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/Menus
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPost(Name = "PostMenu")]
        [ProducesResponseType(typeof(MenuDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody]PostMenuDTO menuDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (menuDTO.RestaurantId.Equals(null)) return BadRequest("Menu is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("New menu can only be added by admin or by restaurant user");

            int userMenus = _menuService.GetUserMenuCount(User.Identity.GetUserId());
            if (!User.IsInRole("premiumUser") && !User.IsInRole("admin") && userMenus >= 1)
            {
                return BadRequest("Regular user can only create 1 menu. Please sign up for premium services to add more.");
            }
            var newMenu = _menuService.AddNewMenu(menuDTO, User.Identity.GetUserId());

            return CreatedAtRoute("GetMenu", new { id = newMenu.MenuId }, newMenu);
        }

        /// <summary>
        /// Update an existing menu
        /// </summary>
        /// <param name="id">ID of menu to update</param>
        /// <param name="menuDTO">Updated object</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Menus/{id}
        ///     {
        ///         ...
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Menu was successfully updated, updated Menu to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Menus/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MenuDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]PostMenuDTO menuDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid fields provided, please double check the parameters");
            if (menuDTO.RestaurantId.Equals(null)) return BadRequest("Menu is not related any Restaurant");
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("Menu can only be updated by admin or by restaurant user");

            var m = _menuService.GetMenuById(id);

            if (m == null) return NotFound();
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
        // DELETE: api/v1/Menus/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("Menu can only be deleted by admin or by restaurant user");
            _menuService.DeleteMenu(id);
            return NoContent();
        }

        /// <summary>
        /// Adds dishes to the menu by id.
        /// </summary>
        /// <param name="id">ID of menu to add Dishes to</param>
        /// <response code="204">MenuDishes were successfully updated</response>
        /// <response code="404">Menu not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Menus/5/Dishes
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}/Dishes")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]int[] dishIds)
        {
            var menuDTO = _menuService.GetMenuById(id);
            if (menuDTO == null) return NotFound();
            if (!IsRestaurantUserOrAdmin(menuDTO.RestaurantId)) return BadRequest("Dishes can only be added by admin or by restaurant user");

            _menuService.UpdateMenuDishes(id, dishIds);

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