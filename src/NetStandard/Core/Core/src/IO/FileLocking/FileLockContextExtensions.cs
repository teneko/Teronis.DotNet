// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.IO.FileLocking
{
    internal static class FileLockContextExtensions
    {
        public static bool IsErroneous(this FileLockContext? fileLockContext)
        {
            if (fileLockContext?.Error is null) {
                return false;
            }

            return true;
        }
    }
}
