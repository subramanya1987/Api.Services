namespace Api.Services.Models.Api
{
    public class HealthCheckResult
    {
        public string? ServiceName { get; set; }
        public bool IsHealthy { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }

        public HealthCheckResult() 
        {
            IsHealthy = true; // Default to healthy
            Message = null;
        }
        public HealthCheckResult(bool isHealthy, string? message, string? serviceName = null, string? error = null)
        {            
            IsHealthy = isHealthy;
            Message = message;
            ServiceName = serviceName;
            Error = error;
        }
    }
}
