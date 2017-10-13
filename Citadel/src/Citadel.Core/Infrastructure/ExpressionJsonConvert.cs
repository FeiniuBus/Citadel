using Citadel.ExpressionJsonSerializer;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Citadel.Infrastructure
{
    public static class ExpressionJsonConvert
    {
        public static string Serialize(Expression<Action> expression, Assembly assembly)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ExpressionJsonConverter(assembly));

            return JsonConvert.SerializeObject(expression, Formatting.Indented, settings);
        }

        public static Expression<Action> Deserialize(string value, Assembly assembly)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ExpressionJsonConverter(assembly));

            return JsonConvert.DeserializeObject<Expression<Action>>(value, settings);
        }
    }
}
