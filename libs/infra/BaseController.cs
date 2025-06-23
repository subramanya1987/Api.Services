using Api.Services.Infra.Exception;
using Api.Services.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System.Diagnostics;
using System.Net;

namespace Api.Services.Infra;                                                                                                                                                                                             
public abstract class BaseController : ControllerBase
{
    private readonly ILogger _logger;

    public BaseController(ILogger logger) =>_logger = logger;

    /// <summary>
    /// Call to ensure that the API is active.
    /// </summary>
    /// <returns> Ok - HTTP status 200</returns>
    [HttpGet("isActive")]
    public IActionResult IsActive() => Ok();

    /// <summary>
    /// Call to ensure that the API is ready to process requests.
    /// </summary>                                                                   
    /// <returns>
    /// Ok - HTTP status 200 if healthy
    /// InternalServerError - HTTP status 500 if not healthy
    /// </returns>
    [HttpGet("isReady")]
    public IActionResult IsReady() => Ok();

    /// <summary>
    /// Helper method to configure a BadRequest response object with the spceified details.
    /// </summary>
    /// <typeparam name="T"> Type for result object</typeparam>
    /// <param name="stopwatch">StopWatch to add exection time to response</param>
    /// <param name="errorMsg">Error Message</param>
    /// <returns>Resposne object of type patameter</returns>
    protected Response<T> ConfigureBadRequest<T>(Stopwatch stopwatch, string errorMsg) =>
        ConfigureBadRequest<T>(stopwatch, new List<string> { errorMsg});

    /// <summary>
    /// Helper method to configure a BadRequest response object with the specified details.
    /// </summary>
    /// <typeparam name="T">Type for result object.</typeparam>
    /// <param name="stopwatch">StopWatch to add execution time status to response.</param>
    /// <param name="errors">List of errors.</param>
    /// <returns>Response object of type parameter</returns>
    protected Response<T> ConfigureBadRequest<T>(Stopwatch stopwatch, IEnumerable<FieldValidationError> errors)
    {
        Response<T> badRequest=new() 
        {
            ResultCode = StatusCodes.Status400BadRequest,
            errorDetail = new ErrorDetail
            {
                ValidationErrors= new List<FieldValidationError>()
            }
        };

        foreach (var error in errors)
        {
            badRequest.errorDetail.ValidationErrors=errors.ToList();
            _logger.LogWarning("{Error}", error);
        }

        stopwatch.Stop();
        badRequest.properties.Add("statTimeInApi",$"{stopwatch.ElapsedMilliseconds} ms");
        return badRequest;
    }

    /// <summary>
    /// Helper method to configure a BadRequest response object with the specified details.
    /// </summary>
    /// <typeparam name="T">Type for result object.</typeparam>
    /// <param name="stopwatch">StopWatch to add execution time status to response.</param>
    /// <param name="errors">List of errors.</param>
    /// <returns>Response object of type parameter</returns>
    protected Response<T> ConfigureBadRequest<T>(Stopwatch stopwatch, IEnumerable<string> errors) =>
        ConfigureBadRequest<T>(stopwatch, errors.Select(e => new FieldValidationError ("request", e )));

    /// <summary>
    /// Helper method to configure a response object with the specified details.
    /// </summary>
    /// <typeparam name="T">Type for result object.</typeparam>
    /// <param name="result">Response Content</param>
    /// <param name="stopWatch">StopWatch to add execution time status to response.</param>
    /// <param name="exception">Exception details if applicable</param>
    /// <param name="warnings">List of warning error message</param>
    /// <returns>ActionResult object</returns>
    protected ActionResult ConfigureResponse<T>(
        T result = default!,
        Stopwatch? stopWatch = null,
        System.Exception? exception = null,
        IEnumerable<FieldValidationError>? warnings = null)
    {
            ActionResult actionResult;
            Response<T> response = new() { Result = result };

            // Add exception time status to response
            if (stopWatch != null)
            {
                stopWatch.Stop();
                response.properties.Add("statTimeInApi", $"{stopWatch.ElapsedMilliseconds} ms");
            }

            if (warnings?.Any() ?? false)
            {
                response.errorDetail = new()
                {
                    ValidationErrors = warnings.ToList()
                };
            }
            if (exception != null)
            {
                response.errorDetail ??= new();

                // Ser error details
                response.errorDetail.SystemMessage = exception?.Message ?? string.Empty;

                if (exception is APIHttpException httpException)
                {
                    // Extended exception thrown by the system
                    _logger.LogWarning(exception, string.Empty, Array.Empty<object>());
                    response.ResultCode = (int)httpException.StatusCode;

                    if (httpException is APIValidationException validationException)
                    {
                        response.errorDetail.ValidationErrors ??= new();
                        response.errorDetail.ValidationErrors.AddRange(validationException.ValidationErrors.ToList());
                    }

                    actionResult = httpException.StatusCode switch
                    {
                        HttpStatusCode.BadRequest => BadRequest(response),
                        _ => StatusCode((int)httpException.StatusCode, response)
                    };
                }
                else
                {
                    // Unhandled exception
                    _logger.LogError(exception, string.Empty, Array.Empty<object>());
                    response.ResultCode = (int)HttpStatusCode.InternalServerError;
                    actionResult = StatusCode((int)HttpStatusCode.InternalServerError, response);
                }
            }
            else
            {
                // No exception, return OK response
                response.ResultCode = (int)HttpStatusCode.OK;
                actionResult = Ok(response);
            }
        return actionResult;
    }

    protected async Task<IActionResult> HandleRequestAsync<TResponse>(string methodName, Func<Task<TResponse>> action)
        where TResponse : class
    {
        return await HandleRequestAsync<TResponse>(methodName, async ()=>(await  action(), null));
    }

    protected async Task<IActionResult> HandleRequestAsync<TResponse>(string methodName, Func<Task<(TResponse response, IEnumerable<FieldValidationError>? warnings)>> action)
    where TResponse : class
    {
        System.Exception? exception = null;
        TResponse? response = null;
        IEnumerable<FieldValidationError>? warnings = null;

        string controllerName= GetType().Name.Replace("Controller", "");
        string operationName = $"{controllerName}Controller.{methodName}";

        using (LogContext.PushProperty("CorrelationId", $"{operationName} {Guid.NewGuid()}"))
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                (response, warnings) = await action();
            }
            catch (System.Exception ex)
            {
                exception = ex;
                _logger.LogError(ex, "{Response}",JsonConvert.SerializeObject(response));
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("Request {OperationName} completed in {ElapsedMilliseconds} ms", operationName, stopwatch.ElapsedMilliseconds);
            }

            return ConfigureResponse(response, stopwatch, exception, warnings); 
        }
    }

    protected virtual IEnumerable<HealthCheckResult> PerformAdditionalHealthChecks() => [];
}

