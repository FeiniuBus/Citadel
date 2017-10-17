using Citadel.Infrastructure;
using Citadel.Logging;
using Citadel.Rabbit.Infrastructure;
using Citadel.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Citadel.Extensions;

namespace Citadel.Rabbit
{
    public class RabbitConsumer : IConsumer
    {
        private readonly IQueue _queue;
        private readonly IModel _channel;
        private readonly IReadOnlyDictionary<string, object> _args;
        private readonly ILogger _logger;

        protected internal RabbitConsumer(string topic, object args, IQueue queue, IModel channel, ILogger<RabbitConsumer> logger)
        {
            Topic = topic;
            _args = args == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string,object>()) : new ReadOnlyDictionary<string, object>(args.Map());
            _queue = queue;
            _channel = channel;
            _logger = logger;
        }

        public IQueue Queue => _queue;
        public string Topic { get; private set; }

        public event DeliverEventHandler OnDelivered;
        public event ShutdownEventHandler OnShutdown;

        public T GetArgumentValue<T>(string key)
        {
            return _args.ContainsKey(key) ? (T)_args[key] : default(T);
        }

        public Task SubscribeAsync()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.ConsumerCancelled += (sender, e) => _logger.LogInformation(new EventLog { Source = nameof(RabbitConsumer), Target="RabbitMQ Consumer", Event = "ConsumerCancelled", Infomations = new { e.ConsumerTag } });
            consumer.Received += (sender, e) => _logger.LogInformation(new EventLog { Source = nameof(RabbitConsumer), Target = "RabbitMQ Consumer", Event = "Received", Infomations = new { e.ConsumerTag, ContentLength = e.Body.Length, e.DeliveryTag, e.Exchange, e.Redelivered, e.RoutingKey } });
            consumer.Registered += (sender, e) => _logger.LogInformation(new EventLog { Source = nameof(RabbitConsumer), Target = "RabbitMQ Consumer", Event = "Registered", Infomations = new { e.ConsumerTag } });
            consumer.Shutdown += (sender, e) => _logger.LogWarning(new EventLog { Source = nameof(RabbitConsumer), Target = "RabbitMQ Consumer", Event = "Shutdown", Infomations = new { e.Cause, e.ClassId, e.MethodId, e.ReplyCode, e.ReplyText } });

            consumer.Received += (sender, e) => OnDelivered?.Invoke(this, new DeliverEventArgs(e.Body, 
                new
                {
                    e.BasicProperties,
                    e.ConsumerTag,
                    e.DeliveryTag,
                    e.Exchange,
                    e.Redelivered,
                    e.RoutingKey,
                },_channel));
            consumer.Shutdown += (sender, e) => OnShutdown?.Invoke(this, new Infrastructure.ShutdownEventArgs(e.Cause, new
            {
                e.ClassId,
                e.Initiator,
                e.MethodId,
                e.ReplyCode,
                e.ReplyText
            }));
            return Task.Run(() =>
            {
                _channel.BasicConsume(_queue.QueueName, false, consumer);
            });
        }
    }
}
