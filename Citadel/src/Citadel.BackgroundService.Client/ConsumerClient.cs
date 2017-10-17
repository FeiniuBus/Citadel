using Citadel.Data;
using Citadel.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Client
{
    public class ConsumerClient
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMessageQueueClientFactory _messageQueueClientFactory;
        private readonly BackgroundServiceOptions _backgroundServiceOptions;
        private readonly ILogger _logger;
        private readonly IMessagePersistenter _messagePersistenter;

        protected IMessageQueueClient MessageQueueClient { get; private set; }
        protected IExchange Exchange { get; private set; }
        protected IQueue Queue { get; private set; }
        protected IConsumer Consumer { get; private set; }

        public ConsumerClient(IDbConnectionFactory dbConnectionFactory, IMessageQueueClientFactory messageQueueClientFactory, BackgroundServiceOptions backgroundServiceOptions, IMessagePersistenter messagePersistenter, ILogger<ConsumerClient> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _messageQueueClientFactory = messageQueueClientFactory;
            _backgroundServiceOptions = backgroundServiceOptions;
            _logger = logger;
            _messagePersistenter = messagePersistenter;
        }

        public async Task InitClientAsync()
        {
            MessageQueueClient = _messageQueueClientFactory.Create();
            Exchange = await MessageQueueClient.DelayExchangeDeclareAsync(_backgroundServiceOptions.Exchange, _backgroundServiceOptions.ExchangeType, true, null);
            var result = await Exchange.QueueDeclareAsync(_backgroundServiceOptions.QueueName, new QueueDeclareOptions(), null);
            if(result.Succeeded == false)
            {
                throw new Exception($"{string.Join(";", result.Errors)}");
            }
            Queue = result.Queue;
            Consumer = Queue.CreateConsumer(_backgroundServiceOptions.Topic, null);
            Consumer.OnDelivered += Consumer_OnDelivered;
            Consumer.OnShutdown += Consumer_OnShutdown;
        }

        private void Consumer_OnShutdown(IConsumer sender, IShutdownEventArgs args)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(ConsumerClient), Target = "Consumer.OnShutdown", args.Cause, args.Infomations }));
        }

        private void Consumer_OnDelivered(IConsumer sender, IDeliverEventArgs args)
        {
            var json = Encoding.UTF8.GetString(args.Content);
            var tranferMessage = JsonConvert.DeserializeObject<TransferMessage>(json);
            try
            {
                var expr = ExpressionJsonConvert.Deserialize(tranferMessage.Body, Assembly.GetExecutingAssembly());
                var act = expr.Compile();
                act.Invoke();
                args.BasicAck();
            }
            catch
            {

            }
        }

        public async Task ConsumeAsync()
        {
            await Consumer.SubscribeAsync();
        }
    }
}
