using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Citadel.Extensions
{
    public static class ILoggerExtension
    {
        public static void LogError(this ILogger logger, object content) => logger.LogError(message: JsonConvert.SerializeObject(content), args: new object[0]);
        public static void LogDebug(this ILogger logger, object content) => logger.LogDebug(message: JsonConvert.SerializeObject(content), args: new object[0]);
        public static void LogCritical(this ILogger logger, object content) => logger.LogCritical(message: JsonConvert.SerializeObject(content), args: new object[0]);
        public static void LogInformation(this ILogger logger, object content) => logger.LogInformation(message: JsonConvert.SerializeObject(content), args: new object[0]);
        public static void LogTrace(this ILogger logger, object content) => logger.LogTrace(message: JsonConvert.SerializeObject(content), args: new object[0]);
        public static void LogWarning(this ILogger logger, object content) => logger.LogWarning(message: JsonConvert.SerializeObject(content), args: new object[0]);
    }
}
