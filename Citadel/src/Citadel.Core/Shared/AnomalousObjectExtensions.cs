using System.Collections.Generic;
using System.Linq;

namespace Citadel.Shared
{
    public static class AnomalousObjectExtensions
    {
        public static IDictionary<string, object> Map(this object value)
        {
            var properties = value.GetType().GetProperties();
            var collection = properties.Select(x => new KeyValuePair<string, object>(x.Name.Replace("__","-") , x.GetValue(value)));
            return collection.ToDictionary(x => x.Key, x => x.Value);
        }

        public static T As<T>(this object obj) where T : class => obj as T;
    }
}
