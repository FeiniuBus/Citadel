using Citadel.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class MessageQueueClient : IMessageQueueClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly IConnection _connection;

        internal MessageQueueClient(ConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _connectionFactory = connectionFactory;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger< MessageQueueClient>();
            _connection = _connectionFactory.CreateConnection();
            _connection.CallbackException += (sender, e) =>
            {
                _logger.LogError(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "CallbackException", e.Detail, Errors = e.Exception.GetErrors() }));
            };
            _connection.ConnectionBlocked += (sender, e) => _logger.LogError(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "ConnectionBlocked", e.Reason}));
            _connection.ConnectionRecoveryError += (sender, e) => _logger.LogError(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "ConnectionRecoveryError", Errors = e.Exception.GetErrors() }));
            _connection.ConnectionShutdown += (sender, e) => _logger.LogError(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "ConnectionShutdown", e.Cause, e.ClassId, e.MethodId, e.ReplyCode, e.ReplyText }));
            _connection.ConnectionUnblocked += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "ConnectionUnblocked" }));
            _connection.RecoverySucceeded += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "RecoverySucceeded" }));
            
        }

        public IMessageQueueClient CreateScope()
        {
            return new MessageQueueClient(_connectionFactory, _loggerFactory);
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
                channel.BasicAcks += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "BasicAcks", e.DeliveryTag, e.Multiple }));
                channel.BasicNacks += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "BasicNacks", e.DeliveryTag, e.Multiple, e.Requeue }));
                channel.BasicRecoverOk += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "BasicRecoverOk" }));
                channel.BasicReturn += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "BasicReturn", ContentLength = e.Body.Length, e.Exchange, e.ReplyCode, e.ReplyText, e.RoutingKey }));
                channel.CallbackException += (sender, e) => _logger.LogError(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "CallbackException", e.Detail, Errors = e.Exception.GetErrors() }));
                channel.FlowControl += (sender, e) => _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "FlowControl", e.Active }));
                channel.ModelShutdown += (sender, e) => _logger.LogWarning(JsonConvert.SerializeObject(new { Source = nameof(MessageQueueClient), Event = "ModelShutdown", e.Cause, e.ClassId, e.MethodId, e.ReplyCode, e.ReplyText }));
                

                var arguments = args == null ? new Dictionary<string, object>() : args.Map();

                channel.ExchangeDeclare(exchangeName, exchangeType, durable, false, arguments);
                return new RabbitExchange(exchangeName, exchangeType, args, _connection, channel, _loggerFactory);
            });
        }

    }
}
