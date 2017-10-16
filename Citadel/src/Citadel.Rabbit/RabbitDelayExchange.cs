using Citadel.Infrastructure;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class RabbitDelayExchange : RabbitExchange
    {
        protected internal RabbitDelayExchange(string exchangeName, string exchangeType, object arguments, IConnection connection, IModel channel, ILoggerFactory loggerFactory) : base(exchangeName, exchangeType, arguments, connection, channel, loggerFactory)
        {
        }

        public async Task DelayAsync<TContent>(string topic, TimeSpan time, TContent content, object arguments, ISerializer serializer = null)
        {
            var body = serializer.Serialize(typeof(TContent), content);
            IBasicProperties basicProperties = _channel.CreateBasicProperties(); 
            if (arguments != null)
            {
                var properties = typeof(IBasicProperties).GetProperties(System.Reflection.BindingFlags.Public)
                    .Where(x => x.CanWrite);
                var propertyNames = properties.Select(x => x.Name);
                var argumentProperties = arguments.GetType().GetProperties().Where(x => propertyNames.Contains(x.Name));
                foreach (var argumentProperty in argumentProperties)
                {
                    var value = argumentProperty.GetValue(arguments);
                    var property = properties.First(x => x.Name == argumentProperty.Name);
                    property.SetValue(basicProperties, value);
                }
            }
            basicProperties.Headers.Add("x-delay", time.Milliseconds);
            await Task.Run(() =>
            {
                _channel.BasicPublish(ExchangeName, topic, basicProperties, body);
            });
        }
    }
}
