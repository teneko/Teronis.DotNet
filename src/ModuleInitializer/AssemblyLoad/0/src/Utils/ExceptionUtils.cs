using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.ModuleInitializer.AssemblyLoad.Utils
{
    public static class ExceptionUtils
    {
        public static string GetMessageWithPrependedType(Exception error, string? message = null) =>
            $"{(message == null ? null : $"{message} ")}{error.GetType()}: {error.Message}";
    }
}
