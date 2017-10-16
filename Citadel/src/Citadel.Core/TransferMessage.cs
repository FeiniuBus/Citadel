using Citadel.Data;
using System.Collections.Generic;

namespace Citadel
{
    public class TransferMessage
    {
        public TransferMessage()
        {
            Claims = new List<MessageClaim>();
        }

        public string MessageId { get; set; }
        public string Body { get; set; }

        public IEnumerable<MessageClaim> Claims { get; set; }
    }
}
