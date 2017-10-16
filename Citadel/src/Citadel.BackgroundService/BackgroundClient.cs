using Citadel.BackgroundService.Data.DomainModel;
using Citadel.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Citadel.BackgroundService
{
    public class BackgroundClient
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMessageQueueClientFactory _messageQueueClientFactory;

        public BackgroundClient(IDbConnectionFactory dbConnectionFactory, IMessageQueueClientFactory messageQueueClientFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _messageQueueClientFactory = messageQueueClientFactory;
        }

        public async Task Enqueue(JobInfo jobInfo, TimeSpan delay)
        {
            var client = _messageQueueClientFactory.Create();
            var exchange = await client.DelayExchangeDeclareAsync("background.delay.exchange", "topic", true, null);
            var job = new Job(jobInfo);
            var jsonStr = JsonConvert.SerializeObject(job);
            var bytes = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            await exchange.PublishAsync("background.job", bytes, new { Headers = new Dictionary<string, object>() { ["x-delay"] = (int)delay.TotalMilliseconds } });
        }
    }
}
