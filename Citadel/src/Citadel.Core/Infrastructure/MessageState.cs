namespace Citadel.Infrastructure
{
    public enum MessageState
    {
        Scheduled,
        Queued,
        Processing,
        Succeed,
        Failed
    }
}
