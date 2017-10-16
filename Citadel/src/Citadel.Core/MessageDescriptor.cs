using System.Collections.Generic;

namespace Citadel
{
    public class MessageDescriptor
    {
        public MessageDescriptor()
        {
            Arguments = new Dictionary<string, object>();
            Claims = new Dictionary<string, string>();
        }
        public string MessageType { get; set; }
        public string Exchange { get; set; }
        public string Topic { get; set; }
        public string ReplyTo { get; set; }
        public byte[] Body { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}
