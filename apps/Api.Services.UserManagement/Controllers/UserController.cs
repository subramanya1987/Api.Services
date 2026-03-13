using Api.Services.Models.Api;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Manager;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Diagnostics;

namespace Api.Services.UserManagement.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiversion=1.0}/[controller]")]
    [ApiController]
    public class UserController :  Api.Services.Infra.BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IUserManager _userManager;

        public UserController(
            IConfiguration configuration,
            ILogger<ApplicationController> logger,
            IUserManager userManager) : base(logger)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all applications.
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(Response<List<UserResponse>>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAllUsers()
        {
            var stopwatch = Stopwatch.StartNew();
            var identifierId = $"UserController.GetAllUsers {Guid.NewGuid().ToString()}";
            using (LogContext.PushProperty("CorrelationId", identifierId))
            {
                _logger.LogInformation("GetAllApplications called with identifier {Identifier}", identifierId);
                Exception? exception = null;
                List<UserResponse>? response = null;
                try
                {
                    response = await _userManager.GetAllUsers();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    _logger.LogError(ex, "Error occurred while fetching GetAllApplications with identifier {Identifier}", identifierId);
                }
                return ConfigureResponse(response, stopwatch, exception);
            }
        }
    }
}
