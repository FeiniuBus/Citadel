using Citadel.Infrastructure;
using Citadel.Rabbit.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Citadel.Rabbit
{
    public class RabbitMQRequest : IMQRequest
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private IBasicProperties _basicProperties;
        private readonly IEnumerable<PropertyInfo> _basicPropertiesProperties;
        private readonly ISerializer _serializer;

        public RabbitMQRequest(string routeKey, Type contentType, object content, IExchange exchange, IConnectionFactory connectionFactory, IConnection connection, ISerializer serializer)
        {
            RouteKey = routeKey;
            ContentType = contentType;
            Content = content;
            Exchange = exchange;
            _connectionFactory = connectionFactory;
            _connection = connection;
            _channel = connection.CreateModel();
            _basicPropertiesProperties = typeof(IBasicProperties).GetProperties();
            _serializer = serializer;
        }

        public IExchange Exchange { get; private set; }

        public string RouteKey { get; private set; }

        public object Content { get; private set; }

        public Type ContentType { get; private set; }

        public event DeliverEventHandler OnMQResponsed;

        public Task Execute()
        {
            var bytes = _serializer.Serialize(ContentType, Content);
            if(string.IsNullOrEmpty(_basicProperties?.ReplyTo) == false && OnMQResponsed != null)
            {
                var connection = _connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueBind(_basicProperties.ReplyTo, Exchange.ExchangeName, RouteKey);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    if(e.BasicProperties.CorrelationId == _basicProperties.CorrelationId)
                    {
                        channel.BasicAck(e.DeliveryTag, false);
                        OnMQResponsed.Invoke(null, new DeliverEventArgs(e.Body, new
                        {
                            e.BasicProperties,
                            e.ConsumerTag,
                            e.DeliveryTag,
                            e.Exchange,
                            e.Redelivered,
                            e.RoutingKey
                        }, _channel));
                    }
                };
                channel.BasicConsume(_basicProperties.ReplyTo, false, consumer);
                _channel.BasicPublish(Exchange.ExchangeName, RouteKey, _basicProperties, bytes);
            }

            return Task.CompletedTask;
        }

        public void SetProperty(string propertyName, object value)
        {
            var property = _basicPropertiesProperties.FirstOrDefault(x => x.Name.ToLower() == propertyName.ToLower());
            if (property == null) throw new ArgumentException($"Property {propertyName} not found.");

            if(_basicProperties == null)
                _basicProperties = _channel.CreateBasicProperties();

            property.SetValue(_basicProperties, value);
        }
    }
}
