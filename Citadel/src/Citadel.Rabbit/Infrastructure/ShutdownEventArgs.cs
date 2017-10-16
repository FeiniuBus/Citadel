using Citadel.Infrastructure;
using Citadel.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Citadel.Rabbit.Infrastructure
{
    public class ShutdownEventArgs : EventArgs, IShutdownEventArgs
    {
        public ShutdownEventArgs(object cause, IDictionary<string, object> infomations = null)
        {
            Cause = cause;
            Infomations = infomations == null ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()) : new ReadOnlyDictionary<string, object>(infomations);
        }

        public ShutdownEventArgs(object cause, object infomations = null) : this(cause, infomations?.Map()) { }

        public object Cause { get; }

        public IReadOnlyDictionary<string, object> Infomations { get; }
    }
}
