using Citadel.Infrastructure;
using Citadel.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class RabbitConsumer : IConsumer
    {
        private readonly IQueue _queue;
        private readonly IModel _channel;
        private readonly IReadOnlyDictionary<string, object> _args;

        protected internal RabbitConsumer(string topic, object args, IQueue queue, IModel channel)
        {
            Topic = topic;
            _args = args == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string,object>()) : new ReadOnlyDictionary<string, object>(args.Map());
            _queue = queue;
            _channel = channel;
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
            consumer.Received += (sender, e) => OnDelivered?.Invoke(this, new DeliverEventArgs(e.Body, 
                new
                {
                    e.BasicProperties,
                    e.ConsumerTag,
                    e.DeliveryTag,
                    e.Exchange,
                    e.Redelivered,
                    e.RoutingKey,
                }));
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
