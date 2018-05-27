using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TaisKohtApi.Models.AccountViewModels;

namespace TaisKohtApi.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/account/")]
    public class SecurityController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<Role> _roleManager;
        private readonly IRequestLogService _requestLogService;

        public SecurityController(SignInManager<User> signInManager, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IConfiguration configuration, ILogger<SecurityController> logger, Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager, IRequestLogService requestLogService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _roleManager = roleManager;
            _requestLogService = requestLogService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            _requestLogService.SaveRequest(user.Id, "POST", "api/v1/login", "Login");
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    var claims = createClaims(user);
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var userRole in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var userClaims = await _userManager.GetClaimsAsync(user); 
                    claims.AddRange(userClaims);
                    var token = createToken(claims);
                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        }
                    );
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return BadRequest("Too many login attempts, try again later.");
                }
                else
                {
                    return BadRequest("Invalid login attempt, errors: " + result);
                }
            }

            return BadRequest("Could not create token.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            _requestLogService.SaveRequest(null, "POST", "api/v1/register", "Register");
            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user == null && ModelState.IsValid)
            {
                var newUser = new User {UserName = registerViewModel.Email, Email = registerViewModel.Email};
                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var currentUser = await _userManager.FindByEmailAsync(newUser.Email);
                    var currentRole = "";
    
                    if (currentUser.Email == "admin@gmail.com")
                    {
                        await AddToRole(currentUser, "admin");
                        currentRole = "admin";
                    }
                    else
                    {
                        await AddToRole(currentUser, "normalUser");
                        currentRole = "normalUser";
                    }
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    _logger.LogInformation(3, "User create a new account with password.");
                    var claims = createClaims(newUser);
                    claims.Add(new Claim(ClaimTypes.Role, currentRole));
                    var userClaims = await _userManager.GetClaimsAsync(newUser);
                    claims.AddRange(userClaims);
                    var token = createToken(claims);
                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        }
                    );

                }
                else
                {
                    return BadRequest("Invalid registration, error: " + result.Errors.First().Description);
                }
                //return failure reasons to frontend
            }
            return BadRequest("This user already exists.");
        }

        private async Task AddToRole(User currentUser, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new Role{Name = role});
            }

            await _userManager.AddToRoleAsync(currentUser, role);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            _requestLogService.SaveRequest(User.Identity.GetUserId(), "POST", "api/v1/logout", "Logout");
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Ok("User successfully logged out.");
        }


        private List<Claim> createClaims(User user)
        {
            var options = new IdentityOptions();
           
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // sub on subject
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // jti on random string
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };
        }

        private JwtSecurityToken createToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                _configuration["Token:Issuer"],
                _configuration["Token:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
        }
    }
}