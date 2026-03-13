namespace Api.Services.Infra.Events
{
    /// <summary>
    /// An EventManage provide the methods that allow processess to intract with an eventing system like Apache Kafa or RabbitMQ etc..
    /// </summarry>
    public interface IEventProducer
    {
        /// <summary>
        /// Publish an event/message of type T
        /// <param name="T">The type of event/message to publish</param>
        /// <param name="topic">The topic associated with the published message</param>
        /// <param name="message">The event/message to publish of type T</param>"
        /// <returns>When successful, true. Otherwise false</returns>
        ///</summary>
        public Task<PublishResults?> PublishAsync<T>(string topic, T message, string? disposition=null);

    }
}
