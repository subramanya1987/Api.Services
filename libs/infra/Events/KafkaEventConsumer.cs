using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Api.Services.Infra.Events;
public abstract class KafkaEventConsumer<T> : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<T> _logger;
    private IConsumer<Ignore, string?> _kafkaConsumer;
    private const int DelayMilliSecounds = 1000;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="kafkaBroker">Kafka broker configuration.</param>
    /// <param name="groupId">Kafka event group Id.</param>
    /// <param name="topic">Kafka topic.</param>
    /// <param name="logger">Logger Instance</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// 
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public KafkaEventConsumer(IConfiguration config, ILogger<T> logger, string? groupId, string? topic)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (string.IsNullOrWhiteSpace(_config["KAFKA_BROKERS"]))
        {
            throw new ArgumentException("KAFKA_BROKERS configuration is required.");
        }
        if (string.IsNullOrWhiteSpace(groupId))
        {
            throw new ArgumentException("KAFKA_GROUP_ID configuration is required." + nameof(groupId));
        }
        if (string.IsNullOrWhiteSpace(topic))
        {
            throw new ArgumentException("KAFKA_TOPIC configuration is required." + nameof(topic));
        }

        var _kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = _config["KAFKA_BROKERS"],
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnablePartitionEof = true
        };

        //if (!string.IsNullOrWhiteSpace(_config["KAFKA_USERNAME"]) && !string.IsNullOrWhiteSpace(_config["KAFKA_PASSWORD"]))
        //{
        //    _kafkaConfig.Set("security.protocol", _config["KFKA_SECURITY_PROTOCOL"] ?? "SASL_PLAINTEXT");
        //    _kafkaConfig.Set("sasl.username", _config["KAFKA_USERNAME"]);
        //    _kafkaConfig.Set("sasl.password", _config["KAFKA_PASSWORD"]);
        //    if (!string.IsNullOrWhiteSpace(_config["KAFKA_MECHANISM"]))
        //    {
        //        _kafkaConfig.Set("sasl.mechanism", _config["KAFKA_MECHANISM"]);
        //    }
        //    if (!string.IsNullOrWhiteSpace(_config["KAFKA_DEBUG"]) && _config["KAFKA_DEBUG"]?.ToLower() == "true")
        //    {
        //        _kafkaConfig.Set("debug", _config["KAFKA_DEBUG"]);
        //    }
        //}

        _kafkaConsumer = new ConsumerBuilder<Ignore, string?>(_kafkaConfig).Build();
        //.SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}"))
        //.SetLogHandler((_, e) => _logger.LogInformation($"Kafka log: {e.Message}"))
        //.Build();
        _kafkaConsumer.Subscribe(topic);
        logger.LogInformation($"Kafka consumer subscribed to topic: {topic}");
    }


    ///<summary>
    ///Called by the framework to execute the background service.
    ///</summary>
    ///<param name="cancelToken"></param>
    ///<returns></returns>
    protected override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        if (_kafkaConsumer == null)        
            return;

        while (!cancelToken.IsCancellationRequested)
        {
            try
            {
                // Consume messages from the Kafka topic

                var msg = _kafkaConsumer.Consume(100);
                if (!string.IsNullOrWhiteSpace(msg?.Message?.Value))
                {
                    await ProcessMessageAsync(msg.Message.Value);
                    _kafkaConsumer.Commit();
                    _logger.LogInformation($"Consumed message '{msg.Message.Value}' at: '{msg.TopicPartitionOffset}'.");
                }
                else
                {
                    await Task.Delay(DelayMilliSecounds, cancelToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning("Operation was cancelled: {Message}", ex.Message);
            }
            catch (ConsumeException ex)
            {
                if (ex.Error.IsFatal)
                {
                    _logger.LogCritical(ex, "Critical error consuming message: {Message}", ex.Message);
                }
                _logger.LogError(ex, "Error consuming message: {Message}, Reason : {reason}", ex.Message, ex.Error.Reason);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while consuming messages: {Message}", ex.Message);
                await Task.Delay(DelayMilliSecounds, cancelToken);
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _kafkaConsumer?.Close();
        _kafkaConsumer?.Dispose();        
    }

    /// <summary>
    /// Process the consumed message asynchronously.
    /// </summary>
    /// <param name="message"> Kafka event message</param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    protected abstract Task ProcessMessageAsync(string message);

}
