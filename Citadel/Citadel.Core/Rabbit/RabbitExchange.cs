using Citadel.Infrastructure;
using Citadel.Internal;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class RabbitExchange : IExchange
    {
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IReadOnlyDictionary<string, object> _args;

        protected internal RabbitExchange(string exchangeName, string exchangeType, object arguments, IConnection connection, IModel channel)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            _args = arguments == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()) : new ReadOnlyDictionary<string, object>(arguments.Map());
            _connection = connection;
            _channel = channel;
        }

        public string ExchangeName { get; private set; }
        public string ExchangeType { get; private set; }

        public T GetArgumentValue<T>(string key)
        {
            return _args.ContainsKey(key) ? (T)_args[key] : default(T);
        }

        public async Task<QueueDeclareResult> QueueDeclareAsync(string queueName, QueueDeclareOptions options, object arguments)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var resp = _channel.QueueDeclare(queueName, options.Durable, options.Exclusive, options.AutoDelete, args == null ? new Dictionary<string, object>() : args.Map());
                    var queue = new RabbitQueue(queueName, options, args, this, _channel);
                    return QueueDeclareResult.Success(queue);
                });
            }
            catch(Exception e)
            {
                return QueueDeclareResult.Failed(e);
            }
        }
        
        public async Task PublishAsync<TContent>(string topic, TContent content, object arguments, ISerializer serializer = null)
        {
            var body = serializer.Serialize(typeof(TContent), content);
            IBasicProperties basicProperties = null;
            if(arguments != null)
            {
                basicProperties = _channel.CreateBasicProperties();
                var properties = typeof(IBasicProperties).GetProperties(System.Reflection.BindingFlags.Public)
                    .Where(x => x.CanWrite);
                var propertyNames = properties.Select(x => x.Name);
                var argumentProperties = arguments.GetType().GetProperties().Where(x => propertyNames.Contains(x.Name));
                foreach(var argumentProperty in argumentProperties)
                {
                    var value = argumentProperty.GetValue(arguments);
                    var property = properties.First(x => x.Name == argumentProperty.Name);
                    property.SetValue(basicProperties, value);
                }
            }
            await Task.Run(() =>
            {
                _channel.BasicPublish(ExchangeName, topic, basicProperties, body);
            });
        }
        
    }
}
