using Api.Services.Infra.Events;
using Api.Services.Infra.Exception;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Manager;
using Newtonsoft.Json;

namespace Api.Services.UserManagement.Consumer;

public class UserListener : KafkaEventConsumer<UserListener>
{
    private readonly IServiceProvider _serviceProvider;
    private ILogger<UserListener> _logger;
    private readonly IConfiguration _config;
    /// <summary>
    /// Construct a UserConsumer instance.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>

    public UserListener(IConfiguration config, ILogger<UserListener> logger,IServiceProvider serviceProvider)
        : base(config, logger, $"{config["KAFKA_GROUP_NAME"]}{nameof(UserListener)}",config["KAFKA_GROUP_TOPIC_NAME_USER"])
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
       
    }
    
    protected override async Task ProcessMessageAsync(string message)
    {        
        try
        {
            using(var scope = _serviceProvider.CreateAsyncScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
                var userData= JsonConvert.DeserializeObject<List<UserResponse>>(message) ?? 
                    throw new APIException($"Unable to Deserialize Kafka message from the User List: { message }");
                await Task.CompletedTask; // Simulate some processing if needed
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError($"UserConsumer.ProcessMessageAsync failed with error : { ex.Message}, StackTrace {ex}");
        }
    }

}