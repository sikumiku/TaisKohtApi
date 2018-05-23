using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
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

        [RequireRequestValue("role")]
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetUsersByRole")]
        [Route("getAllUsersInRole")]
        public IActionResult Get([FromQuery(Name = "role")] string role)
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
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [Route("addRoleToUser")]
        public async Task<IActionResult> CreateAsync([FromQuery(Name = "role, userId")] string role, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
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

        [RequireRequestValue("id")]
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpGet("{id}"), ActionName("GetUser")]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            if (User.IsInRole("admin") || user.Email == IdentityExtensions.GetUserId(User.Identity))
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
            if (User.IsInRole("admin") || user.Email == IdentityExtensions.GetUserId(User.Identity))
            {
                _userService.UpdateUser(id, userDTO);
                return NoContent();
            }
            return StatusCode(403, Json("Unauthorized access."));
        }

        public class RequireRequestValueAttribute : ActionMethodSelectorAttribute
        {
            public RequireRequestValueAttribute(string valueName)
            {
                ValueName = valueName;
            }
            public string ValueName { get; private set; }
            public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
            {
                return (routeContext.RouteData.Values != null);
            }
        }
    }
}