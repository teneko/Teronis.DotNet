using System;

namespace Teronis.Extensions.Logging
{
    public static class ILoggerExtensions
    {
        public static void LogError(this ILogger logger, Exception? error)
        {
            if (error != null) {
                logger.LogError(error, error.Message);
            }
        }

        public static void LogError(this ILogger logger, EventId eventId, Exception error)
        {
            if (error != null) {
                logger.LogError(eventId, error, error.Message);
            }
        }
    }
}
