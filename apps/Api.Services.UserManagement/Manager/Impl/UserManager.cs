using Api.Services.Infra.Cache;
using Api.Services.Infra.Events;
using Api.Services.Infra.Exception;
using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Data;
using Api.Services.UserManagement.Data.Impl;
using Newtonsoft.Json;

namespace Api.Services.UserManagement.Manager.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventProducer _eventProducer;
        private readonly ILogger<UserManager> _logger;
        private readonly IConfiguration _config;
        public UserManager(IUserRepository userRepository,IEventProducer eventProducer,ILogger<UserManager> logger, IConfiguration config)
        {
            _userRepository = userRepository;           
            _eventProducer = eventProducer;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            try
            {
                var res = await _userRepository.GetAllUsers();

                if (res == null || res.Count == 0)
                {
                    _logger.LogWarning("No users found in the database.");
                    return new List<UserResponse>();
                }

                var kafkaTopic = _config["KAFKA_GROUP_TOPIC_NAME_USER"];
                if (string.IsNullOrWhiteSpace(kafkaTopic))
                {
                    _logger.LogError("Kafka Topic configuration is missing or empty.");
                    throw new APIException("Kafka Topic configuration is missing or invalid.");
                }

                string kafkaMessageString = JsonConvert.SerializeObject(res);
                var result = await _eventProducer.PublishAsync(kafkaTopic, kafkaMessageString, "GetAllUsers");
                if (result == null)
                {
                    _logger.LogWarning("Failed to publish user list to Kafka topic.");
                }
                else
                {
                    _logger.LogInformation("User list published successfully to Kafka topic : " + result.messageId);
                }

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching GetAllUsers");
                throw new APIException("An error occurred while fetching users.", ex);
            }
        }
    }
}
