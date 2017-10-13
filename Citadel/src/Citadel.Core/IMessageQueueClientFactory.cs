namespace Citadel
{
    public interface IMessageQueueClientFactory
    {
        IMessageQueueClient Create();
    }
}
