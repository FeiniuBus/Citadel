using System;
using System.Collections.Generic;
using System.Text;

namespace Citadel.Infrastructure
{
    public class QueueDeclareOptions
    {
        /// <summary>
        /// Persistence option
        /// </summary>
        public bool Durable { get; set; } = false;
        /// <summary>
        /// Exclusive option
        /// </summary>
        public bool Exclusive { get; set; } = false;

        public bool AutoDelete { get; set; } = false;

        public object Arguments { get; set; }
    }
}
