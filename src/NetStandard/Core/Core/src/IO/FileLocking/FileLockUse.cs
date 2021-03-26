// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.IO;

namespace Teronis.IO.FileLocking
{

    public struct FileLockUse : IDisposable
    {
        public FileStream FileStream => fileLockContext.FileStream!;

        private readonly FileLockContext fileLockContext;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly string LockId;

        internal FileLockUse(FileLockContext fileLockContext, string lockId)
        {
            this.fileLockContext = fileLockContext ?? throw new ArgumentNullException(nameof(fileLockContext));

            if (fileLockContext.FileStream is null) {
                throw new ArgumentException("File stream context has invalid file stream.");
            }

            LockId = lockId;
        }

        public void Dispose() =>
            fileLockContext.DecreaseLockUse(false, LockId);
    }
}
