using Citadel.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Server
{
    public class BackgroundClient
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMessageQueueClientFactory _messageQueueClientFactory;
        private readonly JobPersistenter _jobPersistenter;
        private readonly BackgroundServiceOptions _backgroundServiceOptions;

        public BackgroundClient(IDbConnectionFactory dbConnectionFactory, IMessageQueueClientFactory messageQueueClientFactory, JobPersistenter jobPersistenter, BackgroundServiceOptions backgroundServiceOptions)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _messageQueueClientFactory = messageQueueClientFactory;
            _jobPersistenter = jobPersistenter;
            _backgroundServiceOptions = backgroundServiceOptions;
        }

        public async Task Enqueue(JobInfo jobInfo)
        {
            var client = _messageQueueClientFactory.Create();
            var exchange = await client.DelayExchangeDeclareAsync(_backgroundServiceOptions.Exchange, _backgroundServiceOptions.ExchangeType, true, null);
            var job = await _jobPersistenter.AddJobAsync(jobInfo);
            var transferMessage = new TransferMessage
            {
                MessageId = job.Message.Id,
                Body = job.Message.Content,
                Claims = job.Message.Claims
            };
            await exchange.PublishAsync(_backgroundServiceOptions.Topic, transferMessage, new { Headers = new Dictionary<string, object>() { ["x-delay"] = (int)jobInfo.DelayTime.TotalMilliseconds } });
        }
    }
}
