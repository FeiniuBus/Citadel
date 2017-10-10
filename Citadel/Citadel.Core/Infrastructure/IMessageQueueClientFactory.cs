namespace Citadel.Infrastructure
{
    public interface IMessageQueueClientFactory
    {
        IMessageQueueClientFactory Create();
    }
}
