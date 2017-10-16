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
        private readonly JobPersistenter _jobPersistenter;

        public BackgroundClient(IDbConnectionFactory dbConnectionFactory, IMessageQueueClientFactory messageQueueClientFactory, JobPersistenter jobPersistenter)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _messageQueueClientFactory = messageQueueClientFactory;
            _jobPersistenter = jobPersistenter;
        }

        public async Task Enqueue(JobInfo jobInfo, TimeSpan delay)
        {
            var client = _messageQueueClientFactory.Create();
            var exchange = await client.DelayExchangeDeclareAsync("background.delay.exchange", "topic", true, null);
            var job = await _jobPersistenter.AddJobAsync(jobInfo);
            var transferMessage = new TransferMessage
            {
                MessageId = job.Message.Id,
                Body = job.Message.Content,
                Claims = job.Message.Claims
            };

            await exchange.PublishAsync("background.job", transferMessage, new { Headers = new Dictionary<string, object>() { ["x-delay"] = (int)delay.TotalMilliseconds } });
        }
    }
}
