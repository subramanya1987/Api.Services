using Api.Services.Infra.Exception;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Api.Services.Infra.Events;

/// <summary>
/// Event manager implement for Apache Kafka.
/// </summary>
internal class KafkaEventProducer : IEventProducer
{
    private readonly IConfiguration _config;

    /// <summary>
    /// The Kafka event producer instance.
    /// </summary>
    private readonly Confluent.Kafka.IProducer<Null, string> _producer;

    /// <summary>
    /// Construct a kafaka Event Manager. Do this by adding a singleton to your projects Program.cs file.
    /// just before the build step, For instance
    /// builder.Services.AddSingleton<Api.Services.Infra.Events.IEventProducer, Api.Services.Infra.Events.KafkaEventProducer>();
    /// Now you can ass an IEventProducer to your Controller's constructors add tthe framework will pass in a reference
    /// </summary>
    /// <param name="config">An reference to an IConfiguration object</param>
    /// <exception cref="ArgumentNullException">When /if config is null</exception>
    public KafkaEventProducer(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        if(string.IsNullOrWhiteSpace(_config["KAFKA_BROKERS"]))
        {
            throw new ArgumentException("KAFKA_BOOTSTRAP_SERVERS configuration is required.");
        }
        ProducerConfig pConfig = new()
        {
            BootstrapServers = _config["KAFKA_BROKERS"],
            AllowAutoCreateTopics =true,
            Acks=Acks.All
        };

        //if (!string.IsNullOrWhiteSpace(_config["KAFKA_USERNAME"]) && !string.IsNullOrWhiteSpace(_config["KAFKA_PSSWORD"]))
        //{
        //    pConfig.Set("security.protocol", _config["KFKA_SECURITY_PROTOCOL"]?? "SASL_PLAINTEXT");
        //    pConfig.Set("sasl.username", _config["KAFKA_USERNAME"]);
        //    pConfig.Set("sasl.password", _config["KAFKA_PSSWORD"]);
        //    if(!string.IsNullOrWhiteSpace(_config["KAFKA_MECHANISM"]))
        //    {
        //        pConfig.Set("sasl.mechanism", _config["KAFKA_MECHANISM"]);
        //    }
        //    if(!string.IsNullOrWhiteSpace(_config["KAFKA_DEBUG"]) && _config["KAFKA_DEBUG"]?.ToLower()=="true")
        //    {
        //        pConfig.Set("debug", _config["KAFKA_DEBUG"]);
        //    }


        //}
        _producer = new ProducerBuilder<Null, string>(pConfig).Build();
    }

    /// <summary>
    /// Asynchronously publish an event/message of type T to a Kafka topic.
    /// <typeparam name="T">The type of event/message to publish</typeparam>
    /// <param name="topic">The topic associated with the published message</param>
    /// <param name="message">The event/message to publish of type T</param>
    /// <exception cref="APIEventPublishException">When an exception is raised</exception>
    /// </summary>
    public async Task<PublishResults?> PublishAsync<T>(string topic, T message, string? disposition = null)
    {
        if(message == null && string.IsNullOrWhiteSpace(disposition))
            throw new ArgumentException("Message and disposition cannot both be null or empty.", nameof(message));

        // publich a string. if an object is passed in, attempt serialize it.
        string? theMessage = null;
        if (typeof(string)!=typeof(T))
        {
            theMessage=JsonSerializer.Serialize(message);
        }
        else
        {
            theMessage = message as string;
        }

        if(theMessage == null)
            throw new ArgumentException("Cannot determine string representation of queued message");

        //publish the message
        var results = new PublishResults();
        try
        {
            var timesStamp = new Timestamp(DateTime.UtcNow);
            var kafkaMessage = new Message<Null, string>
            {
                Value = theMessage,
                Timestamp = timesStamp
            };

            var result = await _producer.ProduceAsync(topic, kafkaMessage);

            if(string.IsNullOrWhiteSpace(result.Topic) || result.Message?.Timestamp == null)
            {
                throw new APIEventPublishException($"Publish response invalid. No Topic, Offset or Message detail.");
            }

            results.theTopic = result.Topic;
            results.messageId = result.Offset.Value;
            results.utcDateTime = result.Message.Timestamp.UtcDateTime;
        }
        catch(APIEventPublishException)
        {
            throw; // rethrow the custom exception if it was already thrown
        }
        catch(ProduceException<Null, string> ex)
        {
            throw new APIEventPublishException($"Failed to publish message to topic '{topic}': {ex.Error.Reason}", ex);
        }
        catch (System.Exception ex)
        {
            throw new APIEventPublishException($"Failed to publish message to topic '{topic}': {ex.Message}", ex);
        }
        return results;
    }

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }

}

