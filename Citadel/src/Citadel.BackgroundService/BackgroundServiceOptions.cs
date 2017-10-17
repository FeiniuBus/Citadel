namespace Citadel.BackgroundService
{
    public class BackgroundServiceOptions
    {
        /// <summary>
        /// BackgroundService Exchange Name.Default : backgroundservice.queue.Must not use default exchange in production environment.
        /// </summary>
        public string Exchange { get; set; } = "backgroundservice.queue";
        /// <summary>
        /// BackgroundService Exchange Type.Default : direct.
        /// </summary>
        public string ExchangeType { get; set; } = "direct";
        /// <summary>
        /// BackgroundService Message Topic.Default : background.job.
        /// </summary>
        public string Topic { get; set; } = "background.job";
        /// <summary>
        /// BackgroundService Queue Name. Default : background.job.queue
        /// </summary>
        public string QueueName { get; set; } = "background.job.queue";
    }
}
