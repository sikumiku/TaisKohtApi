using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/Menus")]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
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
            var menu = _menuService.GetMenuById(id);
            if (menu == null) return NotFound();
            return Ok(menu);
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

            var newMenu = _menuService.AddNewMenu(menuDTO);

            return CreatedAtAction("Get", new { id = newMenu.MenuId }, newMenu);
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
        /// <response code="204">Menu was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/Menus/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult Put(int id, [FromBody]PostMenuDTO menuDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var m = _menuService.GetMenuById(id);

            if (m == null) return NotFound();
            _menuService.UpdateMenu(id, menuDTO);

            return NoContent();
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
            var m = _menuService.GetMenuById(id);
            if (m == null) return NotFound();
            _menuService.DeleteMenu(id);
            return NoContent();
        }
    }
}