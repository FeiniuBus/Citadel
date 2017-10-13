using Citadel.Infrastructure;

namespace Citadel
{
    public interface IQueue
    {
        IExchange Exchange { get;  }

        string QueueName { get;  }
        QueueDeclareOptions Options { get;  }


        T GetArgumentValue<T>(string key);

        IConsumer CreateConsumer(string topic, object arguments);
    }
}
