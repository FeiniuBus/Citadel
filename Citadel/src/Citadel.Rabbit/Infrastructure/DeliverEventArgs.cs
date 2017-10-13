using Citadel.Infrastructure;
using Citadel.Internal;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Citadel.Rabbit.Infrastructure
{
    public class DeliverEventArgs : EventArgs, IDeliverEventArgs
    {
        private readonly IModel _channel;

        public DeliverEventArgs(byte[] content, IDictionary<string, object> properties, IModel channel)
        {
            Content = content;
            _channel = channel;
            Properties = properties == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string,object>()) : new ReadOnlyDictionary<string, object>(properties);
        }

        public DeliverEventArgs(byte[] content, object properties, IModel channel) : this(content, properties?.Map(), channel) { }

        public IReadOnlyDictionary<string, object> Properties { get; private set; }
        public byte[] Content { get; private set; }

        public void BasicAck()
        {
            _channel.BasicAck((ulong)Properties["DeliveryTag"],false);
        }

        public void BasicNack(bool requeue)
        {
            _channel.BasicNack((ulong)Properties["DeliveryTag"], false, requeue);
        }
    }
}
