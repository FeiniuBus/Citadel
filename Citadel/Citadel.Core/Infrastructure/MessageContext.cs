namespace Citadel.Infrastructure
{
    public class MessageContext
    {
        public string Topic { get; set; }
        public byte[] Content { get; set; }
        public long DeliveryTag { get; set; }
    }
}
