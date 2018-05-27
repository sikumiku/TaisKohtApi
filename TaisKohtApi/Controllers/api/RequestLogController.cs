using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TaisKohtApi.Controllers.api
{
    [Produces("application/json")]
    [Route("api/usageData")]
    public class RequestLogController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public RequestLogController(IConfiguration configuration, ILogger<SecurityController> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Gets logs of user
        /// </summary>
        /// <response code="200">Logs successfully retrieved</response> 
        /// <response code="404">Such user does not exist</response>
        /// <response code="429">Too many requests</response>
        /// <response code="500">Internal error, unable to process request</response>
        // GET: api/v1/usageData/{userId}
        [AllowAnonymous]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(List<RequestLog>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public IActionResult GetRequestLogsForUser(string userId)
        {
            throw new NotImplementedException();
            //var user = _userService.GetUserById(userId);
            //if (user == null) { return BadRequest("Incorrect user id provided."); }
            //return Ok(_requestLogService.GetDataForUser(userId));
        }
    }
}