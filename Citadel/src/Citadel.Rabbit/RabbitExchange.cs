using Citadel.Infrastructure;
using Citadel.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class RabbitExchange : IExchange
    {
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IReadOnlyDictionary<string, object> _args;
        private readonly ILoggerFactory _loggerFactory;
 
        protected internal RabbitExchange(string exchangeName, string exchangeType, object arguments, IConnection connection, IModel channel, ILoggerFactory loggerFactory)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            _args = arguments == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()) : new ReadOnlyDictionary<string, object>(arguments.Map());
            _connection = connection;
            _channel = channel;
            _loggerFactory = loggerFactory;
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
                    var resp = _channel.QueueDeclare(queueName, options.Durable, options.Exclusive, options.AutoDelete, arguments == null ? new Dictionary<string, object>() : arguments.Map());
                    var queue = new RabbitQueue(queueName, options, arguments, this, _channel, _loggerFactory);
                    return QueueDeclareResult.Success(queue);
                });
            }
            catch(Exception e)
            {
                return QueueDeclareResult.Failed(e);
            }
        }

        public async Task PublishAsync(string topic, byte[] content, object arguments)
        {
            await Task.Run(() =>
            {
                IBasicProperties basicProperties = _channel.CreateBasicProperties();
                if (arguments != null)
                {
                    var properties = basicProperties.GetType().GetProperties()
                       .Where(x => x.CanWrite);
                    var propertyNames = properties.Select(x => x.Name);
                    var argumentProperties = arguments.GetType().GetProperties().Where(x => propertyNames.Contains(x.Name));
                    foreach (var argumentProperty in argumentProperties)
                    {
                        var value = argumentProperty.GetValue(arguments);

                        if (argumentProperty.Name == "Headers")
                        {
                            basicProperties.Headers = new Dictionary<string, object>();
                            foreach (var item in (Dictionary<string, object>)value)
                            {
                                basicProperties.Headers.Add(item.Key, item.Value);
                            }
                        }
                        else
                        {
                            var property = properties.First(x => x.Name == argumentProperty.Name);
                            property.SetValue(basicProperties, value);
                        }
                    }
                }

                _channel.BasicPublish(ExchangeName, topic, basicProperties, content);
            });
        }

        public Task PublishAsync(string topic, TransferMessage transferMessage, object arguments)
        {
            var json = JsonConvert.SerializeObject(transferMessage);
            var bytes = Encoding.UTF8.GetBytes(json);
            return PublishAsync(topic, bytes, arguments);
        }
    }
}
