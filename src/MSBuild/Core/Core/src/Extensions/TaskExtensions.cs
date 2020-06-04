using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Teronis.MSBuild.Extensions
{
    public static class TaskExtensions
    {
        public static void LogHighImportantMessage(this TaskLoggingHelper logger, string message)
        {
            if (message != null) {
                logger.LogMessage(MessageImportance.High, message);
            }
        }

        public static void LogHighImportantError(this TaskLoggingHelper logger, string errorMessage)
        {
            if (errorMessage != null) {
                logger.LogMessage(MessageImportance.High, errorMessage);
            }
        }
    }
}
