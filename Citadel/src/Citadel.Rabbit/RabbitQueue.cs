using Citadel.Infrastructure;
using Citadel.Shared;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Citadel.Rabbit
{
    public class RabbitQueue : IQueue
    {
        private readonly IModel _channel;
        private readonly IReadOnlyDictionary<string, object> _args;
        private readonly ILoggerFactory _loggerFactory;

        protected internal RabbitQueue(string queueName, QueueDeclareOptions options, object args, IExchange exchange, IModel channel, ILoggerFactory loggerFactory)
        {
            QueueName = queueName;
            Options = options;
            Exchange = exchange;
            _channel = channel;
            _loggerFactory = loggerFactory;
            _args = args == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()) : new ReadOnlyDictionary<string, object>(args.Map());
        }

        public string QueueName { get; private set; }
        public QueueDeclareOptions Options { get; private set; }
        public IExchange Exchange { get; private set; }

        public IConsumer CreateConsumer(string topic, object arguments)
        {
            _channel.QueueBind(QueueName, Exchange.ExchangeName, topic, arguments == null ? new Dictionary<string, object>() : arguments.Map());
            return new RabbitConsumer(topic, arguments, this, _channel, _loggerFactory.CreateLogger<RabbitConsumer>());
        }

        public T GetArgumentValue<T>(string key)
        {
            return _args.ContainsKey(key) ? (T)_args[key] : default(T);
        }
    }
}
