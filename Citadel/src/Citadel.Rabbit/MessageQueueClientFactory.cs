using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Citadel.Rabbit
{
    public class MessageQueueClientFactory : IMessageQueueClientFactory
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILoggerFactory _loggerFactory;

        public MessageQueueClientFactory(ConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _connectionFactory = connectionFactory;
            _loggerFactory = loggerFactory;
        }

        public IMessageQueueClient Create()
        {
            return new MessageQueueClient(_connectionFactory, _loggerFactory);
        }
    }
}
