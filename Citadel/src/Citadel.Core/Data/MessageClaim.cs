using Citadel.Infrastructure;
using System;

namespace Citadel.Data
{
    public class MessageClaim
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string Name { get; set; }
        public ClaimValueType ValueType { get; set; }
        public string Value { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
