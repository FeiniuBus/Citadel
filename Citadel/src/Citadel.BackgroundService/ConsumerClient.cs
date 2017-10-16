using Citadel.BackgroundService.Data.DomainModel;
using Citadel.Data;
using Citadel.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Citadel.BackgroundService
{
    public class ConsumerClient
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMessageQueueClientFactory _messageQueueClientFactory;
        private readonly BackgroundServiceOptions _backgroundServiceOptions;
        private readonly ILogger _logger;

        protected IMessageQueueClient MessageQueueClient { get; private set; }
        protected IExchange Exchange { get; private set; }
        protected IQueue Queue { get; private set; }
        protected IConsumer Consumer { get; private set; }

        public ConsumerClient(IDbConnectionFactory dbConnectionFactory, IMessageQueueClientFactory messageQueueClientFactory, BackgroundServiceOptions backgroundServiceOptions, ILogger<ConsumerClient> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _messageQueueClientFactory = messageQueueClientFactory;
            _backgroundServiceOptions = backgroundServiceOptions;
            _logger = logger;
        }

        public async Task InitClientAsync()
        {
            MessageQueueClient = _messageQueueClientFactory.Create();
            Exchange = await MessageQueueClient.DelayExchangeDeclareAsync("background.delay.exchange", "topic", true, null);
            var result = await Exchange.QueueDeclareAsync(_backgroundServiceOptions.QueueName, new Citadel.Infrastructure.QueueDeclareOptions(), null);
            if(result.Succeeded == false)
            {
                throw new Exception($"{string.Join(";", result.Errors)}");
            }
            Queue = result.Queue;
            Consumer = Queue.CreateConsumer("background.job", null);
            Consumer.OnDelivered += Consumer_OnDelivered;
            Consumer.OnShutdown += Consumer_OnShutdown;
        }

        private void Consumer_OnShutdown(IConsumer sender, IShutdownEventArgs args)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(new { Source = nameof(ConsumerClient), Target = "Consumer.OnShutdown", args.Cause, args.Infomations }));
        }

        private void Consumer_OnDelivered(IConsumer sender, IDeliverEventArgs args)
        {
            var jsonStr = System.Text.Encoding.UTF8.GetString(args.Content);
            var job = JsonConvert.DeserializeObject<Job>(jsonStr);
            var expr = ExpressionJsonConvert.Deserialize(job.MethodCall, Assembly.GetExecutingAssembly());
            var act = expr.Compile();
            act.Invoke();
            args.BasicAck();
        }

        public async Task ConsumeAsync()
        {
            await Consumer.SubscribeAsync();
        }
    }
}
