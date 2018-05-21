using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/Account/")]
    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            SignInManager<User> signInManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger, 
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetUsersByRole")]
        [Route("getAllUsersInRole")]
        public IActionResult GetUsersByRole([FromQuery(Name = "role")] string role)
        {
            return Ok(_userManager.GetUsersInRoleAsync(role));
        } 

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("addRole")]
        public async Task<IActionResult> Post([FromQuery(Name = "role")] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            return Json(_roleManager.Roles);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("deactivate")]
        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            _userService.DeactivateUser(id);
            return NoContent();
        }

        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            if (user.Email == IdentityExtensions.GetUserId(User.Identity))
            {
                return Ok(user);
            }
            return StatusCode(403, Json("Unauthorized access."));
        }

        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]UserDTO userDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = _userService.GetUserById(id);

            if (user == null) return NotFound();
            _userService.UpdateUser(id, userDTO);

            return NoContent();
        }

    }
}