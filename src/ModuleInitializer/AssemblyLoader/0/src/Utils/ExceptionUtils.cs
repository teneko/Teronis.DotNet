// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.ModuleInitializer.AssemblyLoader.Utils
{
    public static class ExceptionUtils
    {
        public static string GetMessageWithPrependedType(Exception error, string? message = null) =>
            $"{(message == null ? null : $"{message} ")}{error.GetType()}: {error.Message}";
    }
}
