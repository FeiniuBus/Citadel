using System;
using System.Collections.Generic;
using System.Text;

namespace Citadel.Infrastructure
{
    public class MessageContext
    {
        public string Topic { get; set; }
        public byte[] Content { get; set; }
    }
}
