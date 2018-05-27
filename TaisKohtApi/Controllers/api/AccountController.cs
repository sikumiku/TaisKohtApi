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
    [Route("api/v1/accounts/")]
    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<Role> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IRequestLogService _requestLogService;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            SignInManager<User> signInManager, Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager,
            ILogger<AccountController> logger, 
            IUserService userService, IRequestLogService requestLogService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _userService = userService;
            _requestLogService = requestLogService;
        }

        /// <summary>
        /// Get all users in specific role.
        /// </summary>
        /// <param name="role">Role of the requested users</param>
        /// <returns>All users in specific role</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">Unauthorized request, all users are visible only to admin access</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/accounts
        [RequireRequestValue("role")]
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetUsersByRole")]
        [Route("getAllUsersInRole")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetUsersByRole([FromQuery(Name = "role")] string role)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/accounts/getAllUsersInRole", "GetUsersByRole");
            var users = _userManager.GetUsersInRoleAsync(role).Result;
            List<UserDTO> userDtos = new List<UserDTO>();
            if (users != null)
            {
                foreach (User user in users)
                {
                    userDtos.Add(UserDTO.CreateFromDomain(user));
                }
                return Ok(userDtos);
            }
            return NotFound();
        }

        /// <summary>
        /// Find user by ID
        /// </summary>
        /// <param name="id">ID of user to return</param>
        /// <returns>User by ID</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/accounts/5
        [RequireRequestValue("id")]
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpGet("{id}"), ActionName("GetUser")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetUser(string id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "GET", "api/v1/accounts/{id}", "GetUser");
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            if (User.IsInRole("admin") || user.UserId == User.Identity.GetUserId())
            {
                return Ok(user);
            }
            return StatusCode(403, "Unauthorized access.");
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/account/addRole
        ///     {
        ///        "role":"premiumUser"
        ///     }
        ///
        /// </remarks>
        /// <param name="role">Name of the role to be added</param>
        /// <returns>All the existing roles</returns>
        /// <response code="204">Role was added successfully</response>
        /// <response code="403">Unauthorized access, role must be added by admin only</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/accounts/addRole
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("addRole")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddRole([FromQuery(Name = "role")] string role)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/accounts/addRole", "AddRole");
            if (role == null) { return BadRequest("Please add role as parameter in order to add role."); }
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new Role{Name = role});
            }

            return NoContent(); 
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/accounts/addRoleToUser
        ///     {
        ///        "role":"premiumUser",
        ///        "email":"user@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="role" name="email">Name of the role to be added and user's e-mail</param>
        /// <returns>User with newly assigned role</returns>
        /// <response code="201">Returns user with newly assigned role</response>
        /// <response code="403">Unauthorized access, role must be added to user by admin only</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // POST: api/v1/accounts/addRoleToUser
        [Authorize(Roles = "admin, premiumUser, normalUser")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        [Route("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromQuery(Name = "role")] string role, [FromQuery(Name = "userId")] string userId)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/accounts/addRoleToUser", "AddRoleToUser");
            if (role == null || userId == null) return BadRequest();
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(role))
            {
                if (User.IsInRole(role))
                {
                    return StatusCode(400, "User is already in this role.");
                }
                if (user.Id == User.Identity.GetUserId() && role != "admin" || User.IsInRole("admin"))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    return StatusCode(403, "Users can only be amended by themselves or by admins.");
                }
            }
            else
            {
                return BadRequest("No such user and/or role exists. Please double check parameters.");
            }
            return CreatedAtRoute(nameof(GetUser), new { id = user.Id }, UserDTO.CreateFromDomain(user));
        }

        /// <summary>
        /// Update an existing user. Users can only be amended by themselves or admin.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/accounts/{id}
        ///     {
        ///         "email":"user@gmail.com",
        ///         "userName":"NewUserName"
        ///     }
        ///
        /// </remarks>
        /// <param name="id" name="UserDTO">ID of user to update and updated UpdateUserDTO object</param>
        /// <response code="204">User was successfully updated, no content to be returned</response>
        /// <response code="400">Faulty request, please review ID and content body</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // PUT: api/v1/accounts/5
        [Authorize(Roles = "admin, normalUser, premiumUser")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUserData(string id, [FromBody]UpdateUserDTO userDTO)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "PUT", "api/v1/accounts/{id}", "UpdateUserData");
            if (!ModelState.IsValid) return BadRequest();
            var user = _userService.GetUserById(id);

            if (user == null) return NotFound();
            if (User.IsInRole("admin") || user.UserId == User.Identity.GetUserId())
            {
                _userService.UpdateUser(id, userDTO);
                return NoContent();
            }
            return StatusCode(403, "Users can only be amended by themselves or by admins.");
        }

        /// <summary>
        /// Deactivates an user. Admin access only.
        /// </summary>
        /// <param name="id">ID of user to deactivate</param>
        /// <response code="204">User was successfully deactivated, no content to be returned</response>
        /// <response code="404">User not found by given ID</response>
        /// <response code="500">Internal error, unable to process request</response>
        // DELETE: api/v1/accounts/5
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteUser(string id)
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "DELETE", "api/v1/accounts/{id}", "DeleteUser");
            var user = _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            _userService.RemoveUser(id);
            return NoContent();
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