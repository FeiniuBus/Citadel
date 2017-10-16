using Citadel.Infrastructure;
using System;
using System.Collections.Generic;

namespace Citadel.Data
{
    public class Message
    {

        public Message()
        {
            Claims = new List<MessageClaim>();
        }

        public string Id { get; set; }
        public string MessageType { get; set; }
        public string Content { get; set; }
        public MessageState State { get; set; }
        public string StateName => State.ToString();
        public DateTime CreationTime { get; set; }

        public List<MessageClaim> Claims { get; set; }
    }
}
