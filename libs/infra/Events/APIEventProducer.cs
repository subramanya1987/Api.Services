using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Events
{
    public class APIEventProducer : IEventProducer
    {
        private readonly IEventProducer _eventProducer;
        private readonly IConfiguration _config;
        /// <summary>
        /// Construct an APIEventProducer instance.
        /// </summary>
        /// <param name="eventProducer">An instance of IEventProducer</param>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public APIEventProducer(IConfiguration config)
        {
            if(config== null)
                   throw new ArgumentNullException(nameof(config));
            _config = config;
            if(string.IsNullOrWhiteSpace(_config["KAFKA_BROKERS"]))
                   throw new ArgumentException("KAFKA_BROKERS configuration is required.");
            
            _eventProducer = new KafkaEventProducer(config);
        }

        /// <summary>
        /// Publish an event/message of type T to a Kafka topic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="disposition"></param>
        /// <returns></returns>
        public async Task<PublishResults?> PublishAsync<T>(string topic, T message, string? disposition = null)
        {
            return await _eventProducer.PublishAsync(topic, message, disposition);
        }
    }
}
