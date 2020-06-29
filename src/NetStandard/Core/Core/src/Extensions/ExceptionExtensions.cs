using System;

namespace Teronis.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ListInnerMessages(this Exception? exception)
        {
            var lines = exception?.Message ?? string.Empty;

            while (exception != null && (exception = exception.InnerException) != null && exception.Message != null)
                lines += "\r\n" + exception.Message;

            return lines;
        }
    }
}
