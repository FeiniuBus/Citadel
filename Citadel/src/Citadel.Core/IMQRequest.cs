using Citadel.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IMQRequest
    {
        IExchange Exchange{ get; }
        /// <summary>
        /// Topic
        /// </summary>
        string RouteKey { get;  }
        object Content { get;  }
        Type ContentType { get; }

        void SetProperty(string propertyName, object value);

        Task Execute();

        event DeliverEventHandler OnMQResponsed;
    }
}
