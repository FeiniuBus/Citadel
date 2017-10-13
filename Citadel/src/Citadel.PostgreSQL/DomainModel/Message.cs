using Citadel.Infrastructure;
using System;

namespace Citadel.Postgre.DomainModel
{
    public class Message : IEntity<string>,IAggregateRoot
    {

        public Message()
        {
            Claims = new TrackedList<MessageClaim>();
        }

        public string Id { get; set; }
        public string MessageType { get; set; }
        public string Content { get; set; }
        public MessageState State { get; set; }
        public string StateName => State.ToString();
        public DateTime CreationTime { get; set; }

        public TrackedList<MessageClaim> Claims { get; set; }
    }
}
