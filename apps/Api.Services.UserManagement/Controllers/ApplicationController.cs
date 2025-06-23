using Api.Services.Infra;
using Api.Services.Models.Api;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Manager;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Api.Services.UserManagement.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiversion=1.0}/[controller]")]    
    [ApiController]
    public class ApplicationController : Api.Services.Infra.BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IApplicationManager _applicationManager;
       
        public ApplicationController(
            IConfiguration configuration,
            ILogger<ApplicationController> logger,
            IApplicationManager applicationManager) :base(logger)
        {
            _configuration = configuration;
            _logger = logger;
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// Get all applications.
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("GetAllApplications")]
        [ProducesResponseType(typeof(Response<List<ApplicationResponse>>),200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAllApplications()
        {
            var stopwatch =  Stopwatch.StartNew();
            var identifierId = $"ApplicationController.GetAllApplications {Guid.NewGuid().ToString()}";
            using(LogContext.PushProperty("CorrelationId", identifierId))
            {
                _logger.LogInformation("GetAllApplications called with identifier {Identifier}", identifierId);
                Exception? exception = null;
                List<ApplicationResponse>? response = null;
                try
                {
                    response = await _applicationManager.GetAllApplications();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    _logger.LogError(ex, "Error occurred while fetching GetAllApplications with identifier {Identifier}", identifierId);
                }
                return ConfigureResponse(response, stopwatch, exception);
            }
        }

        /// <summary>
        /// Get Applications by Id.
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetApplicationById/{applicationId}")]
        [ProducesResponseType(typeof(Response<ApplicationResponse>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetApplicationById(string applicationId)
        {
            var stopwatch = Stopwatch.StartNew();           
            var identifierId = $"ApplicationController.GetApplicationById {Guid.NewGuid().ToString()}";
            using (LogContext.PushProperty("CorrelationId", identifierId))
            {
                Exception? exception = null;
                ApplicationResponse? response = null;
                try
                {
                    response = await _applicationManager.GetApplicationById(applicationId);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    _logger.LogError(ex, "Error occurred while fetching GetApplicationById with identifier {Identifier}", identifierId);
                }
                return ConfigureResponse(response, stopwatch, exception);
            }            
        }

        [HttpGet("RemoveRedisKey/{redisKey}")]
        [ProducesResponseType(typeof(Response<ApplicationResponse>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> RemoveRedisKey(string redisKey)
        {
            var stopwatch = Stopwatch.StartNew();
            var identifierId = $"ApplicationController.RemoveRedisKey {Guid.NewGuid().ToString()}";
            using (LogContext.PushProperty("CorrelationId", identifierId))
            {
                Exception? exception = null;
                bool? response = null;
                try
                {
                    response = await _applicationManager.RemoveRedisKey(redisKey);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    _logger.LogError(ex, "Error occurred while Remove RedisKey with identifier {Identifier}", identifierId);
                }
                return ConfigureResponse(response, stopwatch, exception);
            }
        }
    }
}
