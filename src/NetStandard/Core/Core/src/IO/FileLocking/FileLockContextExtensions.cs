using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Teronis.IO
{

#nullable enable

    internal static class FileLockContextExtensions
    {
        public static bool IsErroneous(this FileLockContext? fileLockContext) {
            if (fileLockContext?.Error is null) {
                return false;
            }

            return true;
        }
    }
}
