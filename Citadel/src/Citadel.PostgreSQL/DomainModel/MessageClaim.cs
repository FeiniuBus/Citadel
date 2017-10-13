using Citadel.Infrastructure;
using Citadel.Postgre.Infrastructure;
using System;

namespace Citadel.Postgre.DomainModel
{
    public class MessageClaim : IEntity<string>
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string Name { get; set; }
        public ClaimValueType ValueType { get; set; }
        public string Value { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
