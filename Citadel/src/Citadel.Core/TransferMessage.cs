using Citadel.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

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
