using System.Collections.Generic;
using System.Linq;

namespace Citadel.Internal
{
    internal static class AnomalousObjectExtensions
    {
        internal static IDictionary<string, object> Map(this object value)
        {
            var properties = value.GetType().GetProperties();
            var collection = properties.Select(x => new KeyValuePair<string, object>(x.Name.Replace("__","-") , x.GetValue(value)));
            return new Dictionary<string, object>(collection);
        }

        internal static T As<T>(this object obj) where T : class => obj as T;
    }
}
