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
using Microsoft.IdentityModel.Tokens;
using TaisKohtApi.Models.AccountViewModels;

namespace TaisKohtApi.Controllers.api
{
    [Produces("application/json")]
    [Route("api/Security")]
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public SecurityController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("getToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
                if (result.Succeeded)
                {
                    var claims = createClaims(user);
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
            }

            return BadRequest("Could not create token.");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> GetToken([FromBody] RegisterViewModel registerViewModel)
        {
            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user == null)
            {
                var newUser = new User {UserName = registerViewModel.Email, Email = registerViewModel.Email};
                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var claims = createClaims(user);
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
            }
            return BadRequest("This user already exists.");
        }


        private List<Claim> createClaims(User user)
        {
            var options = new IdentityOptions();
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