using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
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
    [Produces("application/json")]
    [Route("api/account/")]
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly RoleManager<Role> _roleManager;

        public SecurityController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration, ILogger<SecurityController> logger, RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
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
                        var role = await _userManager.FindByNameAsync(userRole);
                        if (role != null)
                        {
                            var roleClaims = await _userManager.GetClaimsAsync(role);
                            foreach (Claim roleClaim in roleClaims)
                            {
                                claims.Add(roleClaim);
                            }
                        }
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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
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

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Ok("User successfully logged out.");
        }


        private List<Claim> createClaims(User user)
        {
            var options = new IdentityOptions();
           
            //find user from db, if they dont exist, add normalRole, if yes, check for Role, if null, add Normalrole, otherwise add specific role
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // sub on subject
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // jti on random string
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Email),
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