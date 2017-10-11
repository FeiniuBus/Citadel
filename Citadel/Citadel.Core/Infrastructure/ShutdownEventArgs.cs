using Citadel.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Citadel.Infrastructure
{
    public class ShutdownEventArgs : EventArgs
    {
        public ShutdownEventArgs(object cause, IDictionary<string, object> infomations = null)
        {
            Cause = cause;
            Infomations = infomations == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()) : new ReadOnlyDictionary<string, object>(infomations);
        }

        public ShutdownEventArgs(object cause, object infomations = null) : this(cause, infomations?.Map()) { }

        public object Cause { get; }
        
        IReadOnlyDictionary<string, object> Infomations { get; }
    }
}
