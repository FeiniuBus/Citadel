using Citadel.Infrastructure;
using Citadel.Internal;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class MessageQueueClient : IMessageQueueClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;

        internal MessageQueueClient(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
        }

        public IMessageQueueClient CreateScope()
        {
            return new MessageQueueClient(_connectionFactory);
        }

        public async Task<IExchange> DelayExchangeDeclareAsync(string exchangeName, string exchangeType, bool durable, object args)
        {
            return await ExchangeDeclareAsync(exchangeName, "x-delayed-message", durable, new { x__delayed__type = exchangeType });
        }

        public async Task<IExchange> ExchangeDeclareAsync(string exchangeName, string exchangeType, bool durable, object args)
        {
            return await Task.Run(() =>
            {
                var channel = _connection.CreateModel();

                var arguments = args == null ? new Dictionary<string, object>() : args.Map();

                channel.ExchangeDeclare(exchangeName, exchangeType, durable, false, arguments);
                return new RabbitExchange(exchangeName, exchangeType, args, _connection, channel);
            });
        }
    }
}
